using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization.Users.Dto;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Authorization.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input);

        Task<FileDto> GetUsersToExcel(GetUsersToExcelInput input);
        
        Task<List<string>> GetUserExcelColumnsToExcel();

        Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input);

        Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input);

        Task ResetUserSpecificPermissions(EntityDto<long> input);

        Task UpdateUserPermissions(UpdateUserPermissionsInput input);

        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);

        Task DeleteUser(EntityDto<long> input);

        Task UnlockUser(EntityDto<long> input);

        Task<GetUserForEditOutput> GetUserForView(NullableIdDto<long> input);
    }
}