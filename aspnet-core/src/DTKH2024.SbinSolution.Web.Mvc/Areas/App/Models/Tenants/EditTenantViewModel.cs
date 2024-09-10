using System.Collections.Generic;
using DTKH2024.SbinSolution.Editions.Dto;
using DTKH2024.SbinSolution.MultiTenancy.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Tenants
{
    public class EditTenantViewModel
    {
        public TenantEditDto Tenant { get; set; }

        public IReadOnlyList<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public EditTenantViewModel(TenantEditDto tenant, IReadOnlyList<SubscribableEditionComboboxItemDto> editionItems)
        {
            Tenant = tenant;
            EditionItems = editionItems;
        }
    }
}