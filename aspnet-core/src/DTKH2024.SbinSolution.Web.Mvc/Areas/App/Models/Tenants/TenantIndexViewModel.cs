using System.Collections.Generic;
using DTKH2024.SbinSolution.Editions.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}