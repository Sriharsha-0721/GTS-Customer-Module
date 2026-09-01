namespace GTSCustomerAPI.Domain.Entities;

public class BillingCharge
{
    public int BillingChargesId { get; set; }
    public int CustId { get; set; }
    public int ChargeTypeId { get; set; }
    public decimal Charges { get; set; }
    public int? UpdatedUser { get; set; }
}

public class ChargeDetails
{
    public int BillingChargesId { get; set; }
    public string ChargeType { get; set; }
                                     = string.Empty;
    public string Charges { get; set; }
                                     = string.Empty;
}

public class BillingData
{
    public int BillingDataId { get; set; }
    public string ChargeType { get; set; }
                                  = string.Empty;
}