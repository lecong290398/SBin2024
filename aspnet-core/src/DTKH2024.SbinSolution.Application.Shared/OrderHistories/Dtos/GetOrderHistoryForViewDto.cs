namespace DTKH2024.SbinSolution.OrderHistories.Dtos
{
    public class GetOrderHistoryForViewDto
    {
        public OrderHistoryDto OrderHistory { get; set; }

        public string UserName { get; set; }

        public string TransactionBinTransactionCode { get; set; }

        public string WareHouseGiftCode { get; set; }

        public string HistoryTypeName { get; set; }

    }
}