namespace GTSCustomerAPI.Web.Models;

public class CustomerViewModel
{
    public int CustId { get; set; }
    public short MarketCenter { get; set; }
    public int CustNbr { get; set; }
    public int? Account { get; set; }
    public int? Dept { get; set; }
    public string? Name { get; set; }
    public int Route { get; set; }
    public string GID { get; set; } = string.Empty;
    public DateTime? StopDt { get; set; }
    public string? Addr1 { get; set; }
    public string? Addr2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Freq { get; set; }
    public bool OSSFlag { get; set; }
    public string? PONumber { get; set; }
    public string? BillName { get; set; }
    public string? BillAddr { get; set; }
    public string? BillCity { get; set; }
    public string? BillState { get; set; }
    public string? BillZipCod { get; set; }
    public int UpdtUser { get; set; }
    public DateTime UpdtTime { get; set; }
}