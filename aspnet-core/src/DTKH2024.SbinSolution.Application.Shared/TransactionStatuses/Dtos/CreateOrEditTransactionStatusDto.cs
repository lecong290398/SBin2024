using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.TransactionStatuses.Dtos
{
    public class CreateOrEditTransactionStatusDto : EntityDto<int?>
    {

        [StringLength(TransactionStatusConsts.MaxNameLength, MinimumLength = TransactionStatusConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

    }
}