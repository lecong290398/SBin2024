using DTKH2024.SbinSolution.TransactionStatuses.Dtos;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.TransactionStatuses
{
    public class CreateOrEditTransactionStatusModalViewModel
    {
        public CreateOrEditTransactionStatusDto TransactionStatus { get; set; }

        public bool IsEditMode => TransactionStatus.Id.HasValue;
    }
}