namespace GTS.Domain.Entities;

public class CreateCTSSetting
{
    public short MC { get; set; }
    public int CustNbr { get; set; }
    public int CustNo => CustNbr; // Fallback alias if SP expects @CustNo
    public bool PrintIssueStatusFlag { get; set; }
    public bool PrintBornonDateFlag { get; set; }
    public bool NOGFlag { get; set; }
    public string? LabelHeader { get; set; }
}