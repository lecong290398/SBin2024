using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.HistoryTypes.Dtos
{
    public class GetHistoryTypeForEditOutput
    {
        public CreateOrEditHistoryTypeDto HistoryType { get; set; }

    }
}