﻿using DTKH2024.SbinSolution.Sessions.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Layout
{
    public class SubscriptionBarViewModel
    {
        public int SubscriptionExpireNotifyDayCount { get; set; }

        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }

        public string CssClass { get; set; }
    }
}
