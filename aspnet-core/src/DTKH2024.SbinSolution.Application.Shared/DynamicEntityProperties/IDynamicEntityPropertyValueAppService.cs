using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.DynamicEntityProperties.Dto;
using DTKH2024.SbinSolution.DynamicEntityPropertyValues.Dto;

namespace DTKH2024.SbinSolution.DynamicEntityProperties
{
    public interface IDynamicEntityPropertyValueAppService
    {
        Task<DynamicEntityPropertyValueDto> Get(int id);

        Task<ListResultDto<DynamicEntityPropertyValueDto>> GetAll(GetAllInput input);

        Task Add(DynamicEntityPropertyValueDto input);

        Task Update(DynamicEntityPropertyValueDto input);

        Task Delete(int id);

        Task<GetAllDynamicEntityPropertyValuesOutput> GetAllDynamicEntityPropertyValues(GetAllDynamicEntityPropertyValuesInput input);
    }
}
