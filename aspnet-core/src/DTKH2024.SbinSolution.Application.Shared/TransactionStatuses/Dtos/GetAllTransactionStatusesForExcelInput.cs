using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.TransactionStatuses.Dtos
{
    public class GetAllTransactionStatusesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}