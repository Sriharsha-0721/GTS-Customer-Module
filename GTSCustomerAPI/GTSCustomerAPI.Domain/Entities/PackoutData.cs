namespace GTSCustomerAPI.Domain.Entities;

public class PackoutDetails
{
    public int PkoutRestrictId { get; set; }
    public string PkoutRestrict { get; set; } = "";
    public string Item { get; set; } = "";
    public string Color { get; set; } = "";
    public string Size { get; set; } = "";
}

public class PackoutSave
{
    public int CustId { get; set; }
    // Matches SP param @PackoutRestrict
    public string PackoutRestrict { get; set; } = "";
    // Matches SP param @ItemType
    public string ItemType { get; set; } = "";
    public string Color { get; set; } = "";
    public string Size { get; set; } = "";
    public int UserId { get; set; }
}


