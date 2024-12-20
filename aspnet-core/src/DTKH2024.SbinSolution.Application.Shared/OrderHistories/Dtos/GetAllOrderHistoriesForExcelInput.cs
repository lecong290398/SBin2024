﻿using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.OrderHistories.Dtos
{
    public class GetAllOrderHistoriesForExcelInput
    {
        public string Filter { get; set; }

        public string UserNameFilter { get; set; }

        public string TransactionBinTransactionCodeFilter { get; set; }

        public string WareHouseGiftCodeFilter { get; set; }

        public string HistoryTypeNameFilter { get; set; }

    }
}