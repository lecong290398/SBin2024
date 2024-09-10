using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.Brands.Dtos
{
    public class GetBrandForEditOutput
    {
        public CreateOrEditBrandDto Brand { get; set; }

        public string LogoFileName { get; set; }

    }
}