using System.Text.Json.Serialization;

namespace GTSCustomerAPI.Web.Models;

public class WearerViewModel
{
    public int CustId { get; set; }
    public int CustNbr { get; set; }
    public string? CustName { get; set; }
    public int Route { get; set; }
    public string? GID { get; set; }

    public int WearerId { get; set; }
    public string? WearNbr { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Locker { get; set; }
    public string? LockRm { get; set; }

    // SP returns column "Sex" as int 0=F,1=M
    // API serializes as "sexInt"
    // This handles both cases
    [JsonPropertyName("sexInt")]
    public int SexInt { get; set; }

    public bool IsMale => SexInt == 1;
    public string? Message { get; set; }
}