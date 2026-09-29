namespace GTS.Domain.Entities;

public class CTSSettingDetails
{
    public short MC { get; set; }
    public int CustNbr { get; set; }
    public bool PrintIssueStatusFlag { get; set; }
    public bool PrintBornonDateFlag { get; set; }
    public bool NOGFlag { get; set; }
    public string? LabelHeader { get; set; }
}