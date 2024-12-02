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


    public class TransactionDataOffline
    {
        public string TransactionCode { get; set; }
        public int PlasticQuantity { get; set; }
        public int MetalQuantity { get; set; }
        public int OtherQuantity { get; set; }
        public int DeviceId { get; set; }
    }
}
