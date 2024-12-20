﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.TransactionBins.Dtos
{
    public class CreateOrEditTransactionBinDto : EntityDto<int?>
    {

        public int PlastisQuantity { get; set; }

        public decimal PlastisPoint { get; set; }

        public int MetalQuantity { get; set; }

        public decimal MetalPoint { get; set; }

        public int OrtherQuantity { get; set; }

        public decimal ErrorPoint { get; set; }

        public int DeviceId { get; set; }

        public long? UserId { get; set; }

        public int TransactionStatusId { get; set; }

    }

public class CreateTransactionDeviceBinDto : EntityDto<int?>
    {
        public int PlasticQuantity { get; set; }

        public int MetalQuantity { get; set; }

        public int OtherQuantity { get; set; }

        public int DeviceId { get; set; }

        public int TransactionStatusId { get; set; }
    }
}