using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.WareHouseGifts.Dtos
{
    public class GetAllWareHouseGiftsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CodeFilter { get; set; }

        public int? IsUsedFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string ProductPromotionPromotionCodeFilter { get; set; }

    }
}