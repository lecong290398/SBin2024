using DTKH2024.SbinSolution.HistoryTypes.Dtos;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.HistoryTypes
{
    public class CreateOrEditHistoryTypeModalViewModel
    {
        public CreateOrEditHistoryTypeDto HistoryType { get; set; }

        public bool IsEditMode => HistoryType.Id.HasValue;
    }
}