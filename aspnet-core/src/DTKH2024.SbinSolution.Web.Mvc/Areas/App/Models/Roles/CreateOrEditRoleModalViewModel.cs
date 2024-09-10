using Abp.AutoMapper;
using DTKH2024.SbinSolution.Authorization.Roles.Dto;
using DTKH2024.SbinSolution.Web.Areas.App.Models.Common;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}