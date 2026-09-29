namespace GTS.MVC.Models.CTSSettings
{
    public class CTSSettingsViewModel
    {
        public short MC { get; set; }
        public int CustNo { get; set; }
        public string? Customer { get; set; }
        public int Route { get; set; }
        public string? GID { get; set; }

        public bool PrintIssueStatusFlag { get; set; }
        public bool PrintBornonDateFlag { get; set; }
        public bool NOGFlag { get; set; }
        public string? LabelHeader { get; set; }
    }
}