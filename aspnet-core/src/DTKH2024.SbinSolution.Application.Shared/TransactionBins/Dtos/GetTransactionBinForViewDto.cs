namespace DTKH2024.SbinSolution.TransactionBins.Dtos
{
    public class GetTransactionBinForViewDto
    {
        public TransactionBinDto TransactionBin { get; set; }

        public string DeviceName { get; set; }

        public string UserName { get; set; }

        public string TransactionStatusName { get; set; }

    }
}