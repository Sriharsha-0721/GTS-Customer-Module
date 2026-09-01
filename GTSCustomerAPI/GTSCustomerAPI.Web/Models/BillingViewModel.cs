namespace GTSCustomerAPI.Web.Models;

// GET charges — matches ChargeDetailsDto
public class BillingChargeViewModel
{
    public int BillingChargesId { get; set; }
    public int CustId { get; set; }
    public string ChargeType { get; set; } = "";
    public string Charges { get; set; } = "";
    public int ChargeTypeId { get; set; }
}

// GET types dropdown — matches BillingDataDTO
public class BillingDataViewModel
{
    public int BillingDataId { get; set; }
    public string ChargeType { get; set; } = "";
}

// POST save — matches BillingChargeDTO
public class BillingChargeSaveViewModel
{
    public int BillingChargesId { get; set; }
    public int CustId { get; set; }
    public int ChargeTypeId { get; set; }
    public string ChargeType { get; set; } = "";
    public decimal Charges { get; set; }
    public int UpdtUser { get; set; } = 1;
}

// GET rows + POST save — matches PackoutDTO
public class PackoutRowViewModel
{
    public int PkoutRestrictId { get; set; }
    public int CustId { get; set; }
    public string PkoutRestrict { get; set; } = "";
    // Friend uses "Item" not "ItemType"
    public string Item { get; set; } = "";
    public string Color { get; set; } = "";
    public string Size { get; set; } = "";
    public int UpdtUser { get; set; } = 1;
}

