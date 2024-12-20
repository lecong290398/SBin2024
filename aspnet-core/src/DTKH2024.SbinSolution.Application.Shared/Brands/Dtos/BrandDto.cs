﻿using System;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.Brands.Dtos
{
    public class BrandDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? Logo { get; set; }

        public string LogoFileName { get; set; }

        public int ProductCount { get; set; }

    }
}