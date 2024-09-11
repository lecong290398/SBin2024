using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.OrderHistories.Dtos
{
    public class CreateOrEditOrderHistoryDto : EntityDto<int?>
    {

        public string Description { get; set; }

        public string Reason { get; set; }

        public int Point { get; set; }

        public long UserId { get; set; }

        public int? TransactionBinId { get; set; }

        public int? WareHouseGiftId { get; set; }

        public int HistoryTypeId { get; set; }

    }
}