using DTKH2024.SbinSolution.OrderHistories.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.OrderHistories
{
    public class CreateOrEditOrderHistoryModalViewModel
    {
        public CreateOrEditOrderHistoryDto OrderHistory { get; set; }

        public string UserName { get; set; }

        public string TransactionBinTransactionCode { get; set; }

        public string WareHouseGiftCode { get; set; }

        public string HistoryTypeName { get; set; }

        public List<OrderHistoryHistoryTypeLookupTableDto> OrderHistoryHistoryTypeList { get; set; }

        public bool IsEditMode => OrderHistory.Id.HasValue;
    }
}