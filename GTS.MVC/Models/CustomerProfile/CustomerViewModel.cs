using System.Text.Json.Serialization;

namespace GTS.MVC.Models.CustomerProfile
{
    public class CustomerViewModel
    {
        public int CustId { get; set; }

        [JsonPropertyName("CustNbr")]
        public int CustNo { get; set; }

        public string Name { get; set; } = "";

        public int Route { get; set; }

        [JsonPropertyName("GID")]
        public string Gid { get; set; } = "";

        public string PropertyMark { get; set; } = "";

        [JsonPropertyName("OSSFlag")]
        public bool OSSFlag { get; set; }

        [JsonPropertyName("STFlag")]
        public bool STFlag { get; set; }

        [JsonPropertyName("MarketCenter")]
        public int? MC { get; set; }
        public string BillingCom { get; set; } = string.Empty;
        public string PackoutCom { get; set; } = string.Empty;
        public string WashCom { get; set; } = string.Empty;
        public string SoilCom { get; set; } = string.Empty;
        public string DryerCom { get; set; } = string.Empty;
        public string ReceivingCom { get; set; } = string.Empty;
        public string ShippingCom { get; set; } = string.Empty;
        public string DriverCom { get; set; } = string.Empty;
        public string MendCom { get; set; } = string.Empty;
        public string QACom { get; set; } = string.Empty;
        public string CustSrvCom { get; set; } = string.Empty;
        public string OfficeCom { get; set; } = string.Empty;
        public string GenOfficeCom { get; set; } = string.Empty;
        public string MerControlCom { get; set; } = string.Empty;
        public string MainCleanRoomCom { get; set; } = string.Empty;
        public string QAInspCom { get; set; } = string.Empty;
        public string ProdCom { get; set; } = string.Empty;
    }
}