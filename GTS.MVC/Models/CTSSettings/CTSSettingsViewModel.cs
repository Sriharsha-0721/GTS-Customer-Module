namespace GTS.MVC.Models.CTSSettings
{
    public class CTSSettingsViewModel
    {
        // Customer
        public int? MC { get; set; }

        public int? CustNo { get; set; }

        public int? CustID { get; set; }

        public string? Customer { get; set; }

        public string? City { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? Phone { get; set; }

        public string? Contact { get; set; }

        public int? Route { get; set; }

        public string? GID { get; set; }

        // CTS Settings
        public string? ItemCode { get; set; }

        public bool PrintIssueStatusFlag { get; set; }

        public bool PrintBornonDateFlag { get; set; }

        public bool NOGFlag { get; set; }

        public string? LabelHeader { get; set; }
    }
}