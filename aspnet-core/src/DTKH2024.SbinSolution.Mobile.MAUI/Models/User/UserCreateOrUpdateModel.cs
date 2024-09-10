using Abp.AutoMapper;
using DTKH2024.SbinSolution.Authorization.Users.Dto;

namespace DTKH2024.SbinSolution.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(CreateOrUpdateUserInput))]
    public class UserCreateOrUpdateModel : CreateOrUpdateUserInput
    {

    }
}
