using System.Collections.Generic;
using DTKH2024.SbinSolution.DashboardCustomization.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.CustomizableDashboard
{
    public class AddWidgetViewModel
    {
        public List<WidgetOutput> Widgets { get; set; }

        public string DashboardName { get; set; }

        public string PageId { get; set; }
    }
}
