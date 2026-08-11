namespace GTS.MVC.Models.Billing
{
    public class BillingChargeViewModel
    {
        public int BillingChargesId { get; set; }

        public int CustId { get; set; }

        public int ChargeTypeId { get; set; }

        public string ChargeType { get; set; } = "";

        public decimal Charges { get; set; }

        public int UpdtUser { get; set; }
    }
}