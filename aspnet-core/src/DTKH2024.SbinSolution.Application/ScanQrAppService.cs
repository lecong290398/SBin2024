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

            //Mode offline
            if (ContainsHyphen(input.TransactionCode))
            {
                Console.WriteLine("Chuỗi chứa ký tự '-'");
                var dataOffline = DecodeStringToObject(dataQRStr);
                // Check data offline
                if (dataOffline == null)
                {
                    throw new UserFriendlyException("Error. The QR code is invalid.");
                }
                // Check device
                var device = await _deviceRepository.GetAsync(dataOffline.DeviceId);
                if (device == null)
                {
                    throw new UserFriendlyException("Device Not Found !");
                }
                // Check transaction bin
                var transactionCheck = await _transactionBinRepository.FirstOrDefaultAsync(tb => tb.TransactionCode == dataOffline.TransactionCode);
                if (transactionCheck.UserId != null && transactionCheck.UserId != userCurrent.Id && transactionCheck.TransactionStatusId == AppConsts.TransactionStatusIdSuccess)
                {
                    throw new UserFriendlyException("Error. This QR code has already been used on another account.");
                }
                else if (transactionCheck.TransactionStatusId == AppConsts.TransactionStatusIdSuccess)
                {
                    throw new UserFriendlyException("Error. This QR code has already been used.");
                }
                // Create transaction bin
                var inputCreate = new CreateOrEditTransactionBinDto();
                // Calculate point
                inputCreate.MetalPoint = dataOffline.MetalQuantity * device.MetalPoint;
                inputCreate.MetalQuantity = dataOffline.MetalQuantity;
                inputCreate.PlastisPoint = dataOffline.PlasticQuantity * device.PlastisPoint;
                inputCreate.PlastisQuantity = dataOffline.PlasticQuantity;
                inputCreate.ErrorPoint = 10;
                inputCreate.DeviceId = dataOffline.DeviceId;
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
            else // Mode online
            {
                input.TransactionCode = StringEncryption.Decrypt(input.TransactionCode);
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

        private bool ContainsHyphen(string input)
        {
            return input.Contains("-");
        }
        // Hàm kiểm tra chuỗi có đúng định dạng không
        private TransactionDataOffline DecodeStringToObject(string input)
        {
            try
            {
                string[] parts = input.Split('_');

                var data = new TransactionDataOffline
                {
                    TransactionCode = parts[0],
                    PlasticQuantity = int.Parse(parts[1]),
                    MetalQuantity = int.Parse(parts[2]),
                    OtherQuantity = int.Parse(parts[3]),
                    DeviceId = int.Parse(parts[7])
                };
                return data;
            }
            catch (Exception)
            {

                return null;
            }



        }
    }
}
