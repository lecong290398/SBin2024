using System.Collections.Generic;
using DTKH2024.SbinSolution.Editions.Dto;
using DTKH2024.SbinSolution.Security;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Tenants
{
    public class CreateTenantViewModel
    {
        public IReadOnlyList<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public CreateTenantViewModel(IReadOnlyList<SubscribableEditionComboboxItemDto> editionItems)
        {
            EditionItems = editionItems;
        }
    }
}