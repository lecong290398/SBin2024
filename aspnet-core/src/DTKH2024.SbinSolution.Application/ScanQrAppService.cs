using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.Devices;
using DTKH2024.SbinSolution.Extension;
using DTKH2024.SbinSolution.HistoryTypes;
using DTKH2024.SbinSolution.OrderHistories;
using DTKH2024.SbinSolution.ProductPromotions;
using DTKH2024.SbinSolution.ScanQR;
using DTKH2024.SbinSolution.ScanQR.Dto;
using DTKH2024.SbinSolution.TransactionBins;
using DTKH2024.SbinSolution.TransactionBins.Dtos;
using DTKH2024.SbinSolution.WareHouseGifts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DTKH2024.SbinSolution
{
    [AbpAuthorize(AppPermissions.Pages_ScanQR)]
    public class ScanQrAppService : SbinSolutionAppServiceBase, IScanQrAppService
    {
        private readonly IAbpSession _abpSession;
        private readonly IRepository<TransactionBin> _transactionBinRepository;
        private readonly IRepository<HistoryType> _historyTypeRepository;
        private readonly IRepository<OrderHistory> _orderHistoryRepository;
        private readonly IRepository<Device> _deviceRepository;

        public ScanQrAppService(IAbpSession abpSession, IRepository<TransactionBin> transactionBinRepository, IRepository<HistoryType> historyTypeRepository, IRepository<OrderHistory> orderHistoryRepository, IRepository<Device> deviceRepository)
        {
            _abpSession = abpSession;
            _transactionBinRepository = transactionBinRepository;
            _historyTypeRepository = historyTypeRepository;
            _orderHistoryRepository = orderHistoryRepository;
            _deviceRepository = deviceRepository;
        }

        public virtual async Task<int> HandleScanQR(CreateOrEditScanQRDto input)
        {
            // Check user login
            var userId = _abpSession.UserId ?? throw new UserFriendlyException("You are not logged in to the system.");
            var userCurrent = await UserManager.FindByIdAsync(userId.ToString());
            if (userCurrent is null)
            {
                throw new UserFriendlyException(L("UserNotFound"));
            }
            // Decrypt transaction code
            var dataQRStr = StringEncryption.Decrypt(input.TransactionCode);

            // Check if the decrypted transaction code is a valid JSON object
            try
            {
                var transactionData = JsonConvert.DeserializeObject<TransactionDataOffline>(dataQRStr);
                if (transactionData == null)
                {
                    throw new JsonException(); // Throw an exception if json is null
                }
                else
                {
                    Console.WriteLine("Decrypted transaction code is a valid JSON object.");
                    var device = await _deviceRepository.GetAsync(transactionData.deviceId);
                    if (device == null)
                    {
                        throw new UserFriendlyException("Device Not Found !");
                    }
                    // Create transaction bin
                    var inputCreate = new CreateOrEditTransactionBinDto();
                    // Calculate point
                    inputCreate.MetalPoint = transactionData.MetalQuantity * device.MetalPoint;
                    inputCreate.MetalQuantity = transactionData.MetalQuantity;
                    inputCreate.PlastisPoint = transactionData.PlasticQuantity * device.PlastisPoint;
                    inputCreate.PlastisQuantity = transactionData.PlasticQuantity;
                    inputCreate.ErrorPoint = 100;
                    inputCreate.DeviceId = transactionData.deviceId;
                    // Set transaction status wait
                    inputCreate.TransactionStatusId = AppConsts.TransactionStatusIdWait;
                    inputCreate.UserId = device.UserId;
                    var transactionBinOffline = ObjectMapper.Map<TransactionBin>(inputCreate);
                    // Create transaction code
                    transactionBinOffline.TransactionCode = AppConsts.getCodeRandom(AppConsts.keyPerfixTransactionBins);
                    var transactionBinID = await _transactionBinRepository.InsertAndGetIdAsync(transactionBinOffline);
                    // Encrypt transaction code
                    input.TransactionCode = transactionBinOffline.TransactionCode;
                }
            }
            catch (JsonException)
            {
                input.TransactionCode = dataQRStr;
            }

            // Check transaction bin
            var transactionBin = await _transactionBinRepository.FirstOrDefaultAsync(tb => tb.TransactionCode == input.TransactionCode);
            if (transactionBin is null)
            {
                throw new UserFriendlyException("Transaction not found.");
            }
            else if (transactionBin.UserId != null && transactionBin.UserId != userCurrent.Id && transactionBin.TransactionStatusId == AppConsts.TransactionStatusIdSuccess)
            {
                throw new UserFriendlyException("Error. This QR code has already been used on another account.");
            }
            else if (transactionBin.TransactionStatusId == AppConsts.TransactionStatusIdSuccess)
            {
                throw new UserFriendlyException("Error. This QR code has already been used.");

            }
            // Update transaction bin
            transactionBin.TransactionStatusId = AppConsts.TransactionStatusIdSuccess;
            await _transactionBinRepository.UpdateAsync(transactionBin);
            // Calculate point
            var point = (int)(transactionBin.PlastisPoint + transactionBin.MetalPoint + transactionBin.ErrorPoint);
            // Get history type
            var historyType = await _historyTypeRepository.FirstOrDefaultAsync(AppConsts.HistoryType_TichDiem)
                   ?? throw new UserFriendlyException("An error occurred, please try again later.");
            // Create order history
            var orderHistory = new OrderHistory
            {
                Description = historyType.Name + " với giao dịch " + transactionBin.TransactionCode,
                Point = point,
                UserId = userId,
                HistoryTypeId = historyType.Id,
                TransactionBinId = transactionBin.Id,
                Reason = historyType.Description
            };
            await _orderHistoryRepository.InsertAsync(orderHistory);
            // Update point
            userCurrent.Point += point;
            userCurrent.PositivePoint += point;
            await UserManager.UpdateAsync(userCurrent);
            return point;
        }
    }
}
