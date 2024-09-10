using Abp.AutoMapper;
using DTKH2024.SbinSolution.Authorization.Users.Dto;

namespace DTKH2024.SbinSolution.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(UserListDto))]
    public class UserListModel : UserListDto
    {
        public string Photo { get; set; }

        public string FullName => Name + " " + Surname;
    }
}
