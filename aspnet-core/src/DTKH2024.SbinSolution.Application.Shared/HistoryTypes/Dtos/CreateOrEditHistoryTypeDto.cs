using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.HistoryTypes.Dtos
{
    public class CreateOrEditHistoryTypeDto : EntityDto<int?>
    {

        [Required]
        [StringLength(HistoryTypeConsts.MaxNameLength, MinimumLength = HistoryTypeConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

    }
}