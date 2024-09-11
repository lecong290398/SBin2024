using System;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.OrderHistories.Dtos
{
    public class OrderHistoryDto : EntityDto
    {
        public bool IsGive { get; set; }

        public string Description { get; set; }

        public string Reason { get; set; }

        public int Point { get; set; }

        public long UserId { get; set; }

        public int? TransactionBinId { get; set; }

        public int? WareHouseGiftId { get; set; }

        public int HistoryTypeId { get; set; }

    }
}