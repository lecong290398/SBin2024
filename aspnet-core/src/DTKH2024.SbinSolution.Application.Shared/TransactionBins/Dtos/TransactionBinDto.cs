using System;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.TransactionBins.Dtos
{
    public class TransactionBinDto : EntityDto
    {
        public int PlastisQuantity { get; set; }

        public decimal PlastisPoint { get; set; }

        public int MetalQuantity { get; set; }

        public decimal MetalPoint { get; set; }

        public int OrtherQuantity { get; set; }

        public decimal ErrorPoint { get; set; }

        public string TransactionCode { get; set; }

        public int DeviceId { get; set; }

        public long? UserId { get; set; }

        public int TransactionStatusId { get; set; }

    }
}