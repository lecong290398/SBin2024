using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.RankLevels;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.RankLevels;
using DTKH2024.SbinSolution.RankLevels.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

using System.IO;
using System.Linq;
using Abp.Web.Models;
using Abp.UI;
using Abp.IO.Extensions;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RankLevels)]
    public class RankLevelsController : SbinSolutionControllerBase
    {
        private readonly IRankLevelsAppService _rankLevelsAppService;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const long MaxLogoLength = 5242880; //5MB
        private const string MaxLogoLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] LogoAllowedFileTypes = { "jpeg", "jpg", "png" };

        public RankLevelsController(IRankLevelsAppService rankLevelsAppService, ITempFileCacheManager tempFileCacheManager)
        {
            _rankLevelsAppService = rankLevelsAppService;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public ActionResult Index()
        {
            var model = new RankLevelsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RankLevels_Create, AppPermissions.Pages_Administration_RankLevels_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRankLevelForEditOutput getRankLevelForEditOutput;

            if (id.HasValue)
            {
                getRankLevelForEditOutput = await _rankLevelsAppService.GetRankLevelForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRankLevelForEditOutput = new GetRankLevelForEditOutput
                {
                    RankLevel = new CreateOrEditRankLevelDto()
                };
            }

            var viewModel = new CreateOrEditRankLevelModalViewModel()
            {
                RankLevel = getRankLevelForEditOutput.RankLevel,
                LogoFileName = getRankLevelForEditOutput.LogoFileName,
            };

            foreach (var LogoAllowedFileType in LogoAllowedFileTypes)
            {
                viewModel.LogoFileAcceptedTypes += "." + LogoAllowedFileType + ",";
            }

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRankLevelModal(int id)
        {
            var getRankLevelForViewDto = await _rankLevelsAppService.GetRankLevelForView(id);

            var model = new RankLevelViewModel()
            {
                RankLevel = getRankLevelForViewDto.RankLevel
            };

            return PartialView("_ViewRankLevelModal", model);
        }

        public FileUploadCacheOutput UploadLogoFile()
        {
            try
            {
                //Check input
                if (Request.Form.Files.Count == 0)
                {
                    throw new UserFriendlyException(L("NoFileFoundError"));
                }

                var file = Request.Form.Files.First();
                if (file.Length > MaxLogoLength)
                {
                    throw new UserFriendlyException(L("Warn_File_SizeLimit", MaxLogoLengthUserFriendlyValue));
                }

                var fileType = Path.GetExtension(file.FileName).Substring(1);
                if (LogoAllowedFileTypes != null && LogoAllowedFileTypes.Length > 0 && !LogoAllowedFileTypes.Contains(fileType))
                {
                    throw new UserFriendlyException(L("FileNotInAllowedFileTypes", LogoAllowedFileTypes));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var fileToken = Guid.NewGuid().ToString("N");
                _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

                return new FileUploadCacheOutput(fileToken);
            }
            catch (UserFriendlyException ex)
            {
                return new FileUploadCacheOutput(new ErrorInfo(ex.Message));
            }
        }

    }
}