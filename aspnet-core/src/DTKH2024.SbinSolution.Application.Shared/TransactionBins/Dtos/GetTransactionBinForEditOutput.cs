using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.TransactionBins.Dtos
{
    public class GetTransactionBinForEditOutput
    {
        public CreateOrEditTransactionBinDto TransactionBin { get; set; }

        public string DeviceName { get; set; }

        public string UserName { get; set; }

        public string TransactionStatusName { get; set; }

    }
}