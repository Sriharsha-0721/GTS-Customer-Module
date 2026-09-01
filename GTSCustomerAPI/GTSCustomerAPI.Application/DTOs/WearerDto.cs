using System.ComponentModel.DataAnnotations;

namespace GTSCustomerAPI.Application.DTOs;

public class WearerDto
{
    [Required] public int WearerId { get; set; }
    public string Locker { get; set; } = string.Empty;
    public string LockRm { get; set; } = string.Empty;
    public bool IsMale { get; set; }
}

public class NextWearerDto
{
    public int WearerId { get; set; }
    public int CustId { get; set; }
    public int CustNbr { get; set; }
    public string? WearNbr { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Locker { get; set; }
    public string? LockRm { get; set; }
    // SP returns "Sex" as int 0=F 1=M
    // Named SexInt in DTO to be explicit
    public int SexInt { get; set; }
}