using Abp.Application.Services;
using DTKH2024.SbinSolution.ScanQR.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DTKH2024.SbinSolution.ScanQR
{
    public interface IScanQrAppService: IApplicationService
    {
        Task HandleScanQR(CreateOrEditScanQRDto input);
    }
}
