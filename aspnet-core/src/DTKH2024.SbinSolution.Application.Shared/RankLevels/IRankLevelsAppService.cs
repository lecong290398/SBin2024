using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.RankLevels.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.RankLevels
{
    public interface IRankLevelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetRankLevelForViewDto>> GetAll(GetAllRankLevelsInput input);

        Task<GetRankLevelForViewDto> GetRankLevelForView(int id);

        Task<GetRankLevelForEditOutput> GetRankLevelForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRankLevelDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRankLevelsToExcel(GetAllRankLevelsForExcelInput input);

        Task RemoveLogoFile(EntityDto input);

    }
}