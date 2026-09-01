namespace GTSCustomerAPI.Domain.Entities;

public class Wearer
{
    public int WearerId { get; set; }
    public string Locker { get; set; } = string.Empty;
    public string LockRm { get; set; } = string.Empty;
    public string Sex { get; set; } = "F";
}

public class NextWearer
{
    public int WearerId { get; set; }
    public string? WearNbr { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int SexInt { get; set; }
    public string? Locker { get; set; }
    public string? LockRm { get; set; }
    public int CustId { get; set; }
    public int CustNbr { get; set; }
}