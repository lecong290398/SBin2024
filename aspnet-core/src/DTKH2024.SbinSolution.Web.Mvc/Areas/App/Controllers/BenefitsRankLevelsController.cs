using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.BenefitsRankLevels;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.BenefitsRankLevels;
using DTKH2024.SbinSolution.BenefitsRankLevels.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_BenefitsRankLevels)]
    public class BenefitsRankLevelsController : SbinSolutionControllerBase
    {
        private readonly IBenefitsRankLevelsAppService _benefitsRankLevelsAppService;

        public BenefitsRankLevelsController(IBenefitsRankLevelsAppService benefitsRankLevelsAppService)
        {
            _benefitsRankLevelsAppService = benefitsRankLevelsAppService;

        }

        public ActionResult Index()
        {
            var model = new BenefitsRankLevelsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BenefitsRankLevels_Create, AppPermissions.Pages_Administration_BenefitsRankLevels_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetBenefitsRankLevelForEditOutput getBenefitsRankLevelForEditOutput;

            if (id.HasValue)
            {
                getBenefitsRankLevelForEditOutput = await _benefitsRankLevelsAppService.GetBenefitsRankLevelForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getBenefitsRankLevelForEditOutput = new GetBenefitsRankLevelForEditOutput
                {
                    BenefitsRankLevel = new CreateOrEditBenefitsRankLevelDto()
                };
            }

            var viewModel = new CreateOrEditBenefitsRankLevelModalViewModel()
            {
                BenefitsRankLevel = getBenefitsRankLevelForEditOutput.BenefitsRankLevel,
                RankLevelName = getBenefitsRankLevelForEditOutput.RankLevelName,
                BenefitsRankLevelRankLevelList = await _benefitsRankLevelsAppService.GetAllRankLevelForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBenefitsRankLevelModal(int id)
        {
            var getBenefitsRankLevelForViewDto = await _benefitsRankLevelsAppService.GetBenefitsRankLevelForView(id);

            var model = new BenefitsRankLevelViewModel()
            {
                BenefitsRankLevel = getBenefitsRankLevelForViewDto.BenefitsRankLevel
                ,
                RankLevelName = getBenefitsRankLevelForViewDto.RankLevelName

            };

            return PartialView("_ViewBenefitsRankLevelModal", model);
        }

    }
}