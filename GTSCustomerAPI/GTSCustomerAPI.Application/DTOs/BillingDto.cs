namespace GTSCustomerAPI.Application.DTOs;

public class BillingChargeDto
{
    public int BillingChargesId { get; set; }
    public int CustId { get; set; }
    public int ChargeTypeId { get; set; }
    public string ChargeType { get; set; }
                                      = string.Empty;
    public decimal Charges { get; set; }
    public int UpdtUser { get; set; }
}

public class BillingDataDto
{
    public int BillingDataId { get; set; }
    public string ChargeType { get; set; }
                                  = string.Empty;
}

public class ChargeDetailsDto
{
    public int BillingChargesId { get; set; }
    public int CustId { get; set; }
    public string ChargeType { get; set; }
                                     = string.Empty;
    public string Charges { get; set; }
                                     = string.Empty;
}