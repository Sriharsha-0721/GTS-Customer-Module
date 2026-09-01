namespace GTSCustomerAPI.Web.Models;

public class SelectedCustomerViewModel
{
    public int CustId { get; set; }

    public string CustNbr { get; set; } = "";

    public string Name { get; set; } = "";

    public int Route { get; set; }

    public string GID { get; set; } = "";

    // Tells Select Customer where to return
    public string ReturnTo { get; set; } = "";
}
