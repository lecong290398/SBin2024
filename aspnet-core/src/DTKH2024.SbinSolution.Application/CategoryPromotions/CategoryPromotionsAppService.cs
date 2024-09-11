using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.CategoryPromotions.Exporting;
using DTKH2024.SbinSolution.CategoryPromotions.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.CategoryPromotions
{
    [AbpAuthorize(AppPermissions.Pages_Administration_CategoryPromotions)]
    public class CategoryPromotionsAppService : SbinSolutionAppServiceBase, ICategoryPromotionsAppService
    {
        private readonly IRepository<CategoryPromotion> _categoryPromotionRepository;
        private readonly ICategoryPromotionsExcelExporter _categoryPromotionsExcelExporter;

        public CategoryPromotionsAppService(IRepository<CategoryPromotion> categoryPromotionRepository, ICategoryPromotionsExcelExporter categoryPromotionsExcelExporter)
        {
            _categoryPromotionRepository = categoryPromotionRepository;
            _categoryPromotionsExcelExporter = categoryPromotionsExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetCategoryPromotionForViewDto>> GetAll(GetAllCategoryPromotionsInput input)
        {

            var filteredCategoryPromotions = _categoryPromotionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorFilter), e => e.Color.Contains(input.ColorFilter));

            var pagedAndFilteredCategoryPromotions = filteredCategoryPromotions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var categoryPromotions = from o in pagedAndFilteredCategoryPromotions
                                     select new
                                     {

                                         o.Name,
                                         o.Description,
                                         o.Color,
                                         Id = o.Id
                                     };

            var totalCount = await filteredCategoryPromotions.CountAsync();

            var dbList = await categoryPromotions.ToListAsync();
            var results = new List<GetCategoryPromotionForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCategoryPromotionForViewDto()
                {
                    CategoryPromotion = new CategoryPromotionDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Color = o.Color,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCategoryPromotionForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetCategoryPromotionForViewDto> GetCategoryPromotionForView(int id)
        {
            var categoryPromotion = await _categoryPromotionRepository.GetAsync(id);

            var output = new GetCategoryPromotionForViewDto { CategoryPromotion = ObjectMapper.Map<CategoryPromotionDto>(categoryPromotion) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CategoryPromotions_Edit)]
        public virtual async Task<GetCategoryPromotionForEditOutput> GetCategoryPromotionForEdit(EntityDto input)
        {
            var categoryPromotion = await _categoryPromotionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCategoryPromotionForEditOutput { CategoryPromotion = ObjectMapper.Map<CreateOrEditCategoryPromotionDto>(categoryPromotion) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCategoryPromotionDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CategoryPromotions_Create)]
        protected virtual async Task Create(CreateOrEditCategoryPromotionDto input)
        {
            var categoryPromotion = ObjectMapper.Map<CategoryPromotion>(input);

            await _categoryPromotionRepository.InsertAsync(categoryPromotion);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CategoryPromotions_Edit)]
        protected virtual async Task Update(CreateOrEditCategoryPromotionDto input)
        {
            var categoryPromotion = await _categoryPromotionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, categoryPromotion);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CategoryPromotions_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _categoryPromotionRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetCategoryPromotionsToExcel(GetAllCategoryPromotionsForExcelInput input)
        {

            var filteredCategoryPromotions = _categoryPromotionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorFilter), e => e.Color.Contains(input.ColorFilter));

            var query = (from o in filteredCategoryPromotions
                         select new GetCategoryPromotionForViewDto()
                         {
                             CategoryPromotion = new CategoryPromotionDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Color = o.Color,
                                 Id = o.Id
                             }
                         });

            var categoryPromotionListDtos = await query.ToListAsync();

            return _categoryPromotionsExcelExporter.ExportToFile(categoryPromotionListDtos);
        }

    }
}