using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.HistoryTypes.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.HistoryTypes
{
    public interface IHistoryTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetHistoryTypeForViewDto>> GetAll(GetAllHistoryTypesInput input);

        Task<GetHistoryTypeForViewDto> GetHistoryTypeForView(int id);

        Task<GetHistoryTypeForEditOutput> GetHistoryTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditHistoryTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetHistoryTypesToExcel(GetAllHistoryTypesForExcelInput input);

    }
}