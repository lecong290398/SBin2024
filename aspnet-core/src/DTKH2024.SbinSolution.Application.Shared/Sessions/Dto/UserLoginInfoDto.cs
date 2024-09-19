using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using System.Collections.Generic;

namespace DTKH2024.SbinSolution.Sessions.Dto
{
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string ProfilePictureId { get; set; }

        public int Point { get; set; }
        public int PositivePoint { get; set; }
        public  ICollection<UserRole> Roles { get; set; }
        public string RankName { get; set; }
    }
}
