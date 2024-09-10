using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Users
{
    public class UserLoginAttemptsViewModel
    {
        public List<ComboboxItemDto> LoginAttemptResults { get; set; }
    }
}