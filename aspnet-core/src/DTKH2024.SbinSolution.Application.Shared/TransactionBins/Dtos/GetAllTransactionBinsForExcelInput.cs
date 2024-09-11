using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.TransactionBins.Dtos
{
    public class GetAllTransactionBinsForExcelInput
    {
        public string Filter { get; set; }

        public string TransactionCodeFilter { get; set; }

        public string DeviceNameFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string TransactionStatusNameFilter { get; set; }

    }
}