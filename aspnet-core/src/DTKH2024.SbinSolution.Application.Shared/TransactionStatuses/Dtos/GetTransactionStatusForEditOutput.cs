using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.TransactionStatuses.Dtos
{
    public class GetTransactionStatusForEditOutput
    {
        public CreateOrEditTransactionStatusDto TransactionStatus { get; set; }

    }
}