namespace GTS.Application.DTOs
{
    public class BillingChargeDTO
    {
        public int BillingChargesId { get; set; }
        public int CustId { get; set; }
        public int ChargeTypeId { get; set; }
        public string ChargeType { get; set; } = string.Empty;

        public decimal Charges { get; set; }

        public int UpdtUser { get; set; }
    }
}