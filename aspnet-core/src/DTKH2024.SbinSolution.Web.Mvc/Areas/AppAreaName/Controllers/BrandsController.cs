using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Brands;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.Brands;
using DTKH2024.SbinSolution.Brands.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

using System.IO;
using System.Linq;
using Abp.Web.Models;
using Abp.UI;
using Abp.IO.Extensions;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Brands)]
    public class BrandsController : SbinSolutionControllerBase
    {
        private readonly IBrandsAppService _brandsAppService;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const long MaxLogoLength = 5242880; //5MB
        private const string MaxLogoLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] LogoAllowedFileTypes = { "jpeg", "jpg", "png" };

        public BrandsController(IBrandsAppService brandsAppService, ITempFileCacheManager tempFileCacheManager)
        {
            _brandsAppService = brandsAppService;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public ActionResult Index()
        {
            var model = new BrandsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Brands_Create, AppPermissions.Pages_Administration_Brands_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetBrandForEditOutput getBrandForEditOutput;

            if (id.HasValue)
            {
                getBrandForEditOutput = await _brandsAppService.GetBrandForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getBrandForEditOutput = new GetBrandForEditOutput
                {
                    Brand = new CreateOrEditBrandDto()
                };
            }

            var viewModel = new CreateOrEditBrandModalViewModel()
            {
                Brand = getBrandForEditOutput.Brand,
                LogoFileName = getBrandForEditOutput.LogoFileName,
            };

            foreach (var LogoAllowedFileType in LogoAllowedFileTypes)
            {
                viewModel.LogoFileAcceptedTypes += "." + LogoAllowedFileType + ",";
            }

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBrandModal(int id)
        {
            var getBrandForViewDto = await _brandsAppService.GetBrandForView(id);

            var model = new BrandViewModel()
            {
                Brand = getBrandForViewDto.Brand
            };

            return PartialView("_ViewBrandModal", model);
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