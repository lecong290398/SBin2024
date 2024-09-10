using System;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.TransactionStatuses.Dtos
{
    public class TransactionStatusDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

    }
}