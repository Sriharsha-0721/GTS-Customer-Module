namespace GTS.MVC.Models.Billing
{
    public class BillingViewModel
    {
        public List<BillingChargeViewModel> Charges { get; set; } = new();
        public List<BillingDataViewModel> ChargeTypes { get; set; } = new();
        public string BillingCom { get; set; } = string.Empty;
    }
}