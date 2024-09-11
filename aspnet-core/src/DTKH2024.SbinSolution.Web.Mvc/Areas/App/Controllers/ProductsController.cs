using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.Products;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.Products;
using DTKH2024.SbinSolution.Products.Dtos;
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
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Products)]
    public class ProductsController : SbinSolutionControllerBase
    {
        private readonly IProductsAppService _productsAppService;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const long MaxImageLength = 5242880; //5MB
        private const string MaxImageLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] ImageAllowedFileTypes = { "jpeg", "jpg", "png" };

        public ProductsController(IProductsAppService productsAppService, ITempFileCacheManager tempFileCacheManager)
        {
            _productsAppService = productsAppService;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public ActionResult Index()
        {
            var model = new ProductsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Products_Create, AppPermissions.Pages_Administration_Products_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetProductForEditOutput getProductForEditOutput;

            if (id.HasValue)
            {
                getProductForEditOutput = await _productsAppService.GetProductForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getProductForEditOutput = new GetProductForEditOutput
                {
                    Product = new CreateOrEditProductDto()
                };
            }

            var viewModel = new CreateOrEditProductModalViewModel()
            {
                Product = getProductForEditOutput.Product,
                ProductTypeName = getProductForEditOutput.ProductTypeName,
                BrandName = getProductForEditOutput.BrandName,
                ProductProductTypeList = await _productsAppService.GetAllProductTypeForTableDropdown(),
                ProductBrandList = await _productsAppService.GetAllBrandForTableDropdown(),
                ImageFileName = getProductForEditOutput.ImageFileName,
            };

            foreach (var ImageAllowedFileType in ImageAllowedFileTypes)
            {
                viewModel.ImageFileAcceptedTypes += "." + ImageAllowedFileType + ",";
            }

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductModal(int id)
        {
            var getProductForViewDto = await _productsAppService.GetProductForView(id);

            var model = new ProductViewModel()
            {
                Product = getProductForViewDto.Product
                ,
                ProductTypeName = getProductForViewDto.ProductTypeName

                ,
                BrandName = getProductForViewDto.BrandName

            };

            return PartialView("_ViewProductModal", model);
        }

        public FileUploadCacheOutput UploadImageFile()
        {
            try
            {
                //Check input
                if (Request.Form.Files.Count == 0)
                {
                    throw new UserFriendlyException(L("NoFileFoundError"));
                }

                var file = Request.Form.Files.First();
                if (file.Length > MaxImageLength)
                {
                    throw new UserFriendlyException(L("Warn_File_SizeLimit", MaxImageLengthUserFriendlyValue));
                }

                var fileType = Path.GetExtension(file.FileName).Substring(1);
                if (ImageAllowedFileTypes != null && ImageAllowedFileTypes.Length > 0 && !ImageAllowedFileTypes.Contains(fileType))
                {
                    throw new UserFriendlyException(L("FileNotInAllowedFileTypes", ImageAllowedFileTypes));
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