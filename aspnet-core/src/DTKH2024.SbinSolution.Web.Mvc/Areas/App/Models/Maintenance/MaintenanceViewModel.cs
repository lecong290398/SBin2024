using System.Collections.Generic;
using DTKH2024.SbinSolution.Caching.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
        
        public bool CanClearAllCaches { get; set; }
    }
}