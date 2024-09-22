using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTKH2024.SbinSolution.ScanQR.Dto
{
    public class CreateOrEditScanQRDto : EntityDto<int?>
    {
        public string TransactionCode { get; set; }
    }
}
