using System.Collections.Generic;
using DTKH2024.SbinSolution.Authorization.Delegation;
using DTKH2024.SbinSolution.Authorization.Users.Delegation.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }

        public List<UserDelegationDto> UserDelegations { get; set; }

        public string CssClass { get; set; }
    }
}
