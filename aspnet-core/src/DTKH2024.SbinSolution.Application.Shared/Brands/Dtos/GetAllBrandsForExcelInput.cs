using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.Brands.Dtos
{
    public class GetAllBrandsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}