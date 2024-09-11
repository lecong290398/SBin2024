using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.OrderHistories.Dtos
{
    public class GetOrderHistoryForEditOutput
    {
        public CreateOrEditOrderHistoryDto OrderHistory { get; set; }

        public string UserName { get; set; }

        public string TransactionBinTransactionCode { get; set; }

        public string WareHouseGiftCode { get; set; }

        public string HistoryTypeName { get; set; }

    }
}