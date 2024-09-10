namespace DTKH2024.SbinSolution.Web.Models.Payment
{
    public class ContinuePaymentModel
    {
        public long PaymentId { get; set; }
        
        public string Gateway { get; set; }
        
        public bool RecurringPaymentEnabled { get; set; }
    }
}
