using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.ProductTypes.Dtos
{
    public class GetProductTypeForEditOutput
    {
        public CreateOrEditProductTypeDto ProductType { get; set; }

    }
}