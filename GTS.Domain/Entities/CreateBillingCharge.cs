namespace GTS.Domain.Entities
{
    public class CreateBillingCharge
    {
        public int BillingChargesId { get; set; }

        public int CustId { get; set; }

        public int ChargeTypeId { get; set; }

        public decimal Charges { get; set; }

        public int UpdtUser { get; set; }
    }
}