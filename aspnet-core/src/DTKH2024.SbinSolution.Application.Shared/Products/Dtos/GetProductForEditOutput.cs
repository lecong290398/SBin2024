using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.Products.Dtos
{
    public class GetProductForEditOutput
    {
        public CreateOrEditProductDto Product { get; set; }

        public string ProductTypeName { get; set; }

        public string BrandName { get; set; }

        public string ImageFileName { get; set; }

    }
}