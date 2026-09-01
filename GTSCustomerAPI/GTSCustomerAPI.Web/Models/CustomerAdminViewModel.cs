namespace GTSCustomerAPI.Web.Models;

public class CustomerAdminViewModel
{
    // Customer identity (from session — readonly display)
    public int CustId { get; set; }
    public int CustNbr { get; set; }
    public string? Name { get; set; }
    public int Route { get; set; }
    public string? GID { get; set; }

    // Editable flags
    public bool OSSFlag { get; set; }
    public bool STFlag { get; set; }
}