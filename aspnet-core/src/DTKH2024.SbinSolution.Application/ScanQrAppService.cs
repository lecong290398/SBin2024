using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.Extension;
using DTKH2024.SbinSolution.HistoryTypes;
using DTKH2024.SbinSolution.OrderHistories;
using DTKH2024.SbinSolution.ProductPromotions;
using DTKH2024.SbinSolution.ScanQR;
using DTKH2024.SbinSolution.ScanQR.Dto;
using DTKH2024.SbinSolution.TransactionBins;
using DTKH2024.SbinSolution.WareHouseGifts;
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

        public ScanQrAppService(IAbpSession abpSession, IRepository<TransactionBin> transactionBinRepository, IRepository<HistoryType> historyTypeRepository, IRepository<OrderHistory> orderHistoryRepository)
        {
            _abpSession = abpSession;
            _transactionBinRepository = transactionBinRepository;
            _historyTypeRepository = historyTypeRepository;
            _orderHistoryRepository = orderHistoryRepository;
        }

        public virtual async Task HandleScanQR(CreateOrEditScanQRDto input)
        {
            // Check user login
            var userId = _abpSession.UserId ?? throw new UserFriendlyException("You are not logged in to the system.");
            var userCurrent = await UserManager.FindByIdAsync(userId.ToString());
            if (userCurrent is null)
            {
                throw new UserFriendlyException(L("UserNotFound"));
            }
            // Decrypt transaction code
            input.TransactionCode = StringEncryption.Decrypt(input.TransactionCode);
            // Check transaction bin
            var transactionBin = await _transactionBinRepository.FirstOrDefaultAsync(tb => tb.TransactionCode == input.TransactionCode);
            if (transactionBin is null)
            {
                throw new UserFriendlyException("Transaction not found.");
            }
            else if (transactionBin.UserId != null && transactionBin.UserId != userCurrent.Id)
            {
                throw new UserFriendlyException("Error. This QR code has already been used on another account.");
            }
            // Update transaction bin
            transactionBin.TransactionStatusId = AppConsts.TransactionStatusIdSuccess;
            transactionBin.UserId = userCurrent.Id;
            await _transactionBinRepository.UpdateAsync(transactionBin);
            // Calculate point
            var point = (int)(transactionBin.PlastisPoint + transactionBin.MetalPoint + transactionBin.ErrorPoint); 
            // Get history type
            var historyType = await _historyTypeRepository.FirstOrDefaultAsync(AppConsts.HistoryType_DoiQua)
                   ?? throw new UserFriendlyException("An error occurred, please try again later.");
            // Create order history
            var orderHistory = new OrderHistory
            {
                Description = historyType.Name,
                Point = point,
                UserId = userId,
                HistoryTypeId = historyType.Id,
                TransactionBinId = transactionBin.Id
            };
            await _orderHistoryRepository.InsertAsync(orderHistory);
            // Update point
            userCurrent.Point += point;
            userCurrent.PositivePoint += point;
            await UserManager.UpdateAsync(userCurrent);
        }
    }
}
