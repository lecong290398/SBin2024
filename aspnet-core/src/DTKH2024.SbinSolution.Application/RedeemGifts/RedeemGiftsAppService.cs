using Abp.Authorization;
using DTKH2024.SbinSolution.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTKH2024.SbinSolution.RedeemGifts
{
    [AbpAuthorize(AppPermissions.Pages_RedeemGifts)]

    public class RedeemGiftsAppService : IRedeemGiftsAppService
    {
        public RedeemGiftsAppService()
        {
        }
    }
}
