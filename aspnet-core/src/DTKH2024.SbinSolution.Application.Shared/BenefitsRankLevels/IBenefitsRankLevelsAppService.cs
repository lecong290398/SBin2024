using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.BenefitsRankLevels.Dtos;
using DTKH2024.SbinSolution.Dto;
using System.Collections.Generic;

namespace DTKH2024.SbinSolution.BenefitsRankLevels
{
    public interface IBenefitsRankLevelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBenefitsRankLevelForViewDto>> GetAll(GetAllBenefitsRankLevelsInput input);

        Task<GetBenefitsRankLevelForViewDto> GetBenefitsRankLevelForView(int id);

        Task<GetBenefitsRankLevelForEditOutput> GetBenefitsRankLevelForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditBenefitsRankLevelDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetBenefitsRankLevelsToExcel(GetAllBenefitsRankLevelsForExcelInput input);

        Task<List<BenefitsRankLevelRankLevelLookupTableDto>> GetAllRankLevelForTableDropdown();

    }
}