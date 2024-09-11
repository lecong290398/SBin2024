using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.ProductTypes.Dtos
{
    public class CreateOrEditProductTypeDto : EntityDto<int?>
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

    }
}