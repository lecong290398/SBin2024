﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Editions
{
    public class MoveTenantsToAnotherEditionViewModel
    {
        public int EditionId { get; set; }

        public int TenantCount { get; set; }

        public IReadOnlyList<ComboboxItemDto> EditionItems { get; set; }
    }
}