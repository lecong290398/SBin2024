using Abp.Authorization;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.ScanQR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTKH2024.SbinSolution
{
    [AbpAuthorize(AppPermissions.Pages_ScanQR)]
    public class ScanQrAppService : IScanQrAppService
    {
        public ScanQrAppService()
        {
        }
    }
}
