namespace GTSCustomerAPI.Web.Models;

// GET rows + POST save — matches PackoutDTO
public class PackoutViewModel
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