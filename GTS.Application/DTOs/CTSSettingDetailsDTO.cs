namespace GTS.Application.DTOs
{
    public class CTSSettingDetailsDTO
    {
        public short MarketCenter { get; set; }
        public int CustNbr { get; set; }
        public bool PrintIssueStatusFlag { get; set; }
        public bool PrintBornonDateFlag { get; set; }
        public bool NOGFlag { get; set; }
        public string? LabelHeader { get; set; }
    }
}