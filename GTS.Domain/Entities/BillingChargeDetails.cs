namespace GTS.Domain.Entities
{
    public class BillingChargeDetails
    {
        public int BillingChargesId { get; set; }

        public string ChargeType { get; set; } = string.Empty;

        public decimal Charges { get; set; }
    }
}