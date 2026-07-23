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

        public bool OSSFlag { get; set; }

        public bool STFlag { get; set; }

        [JsonPropertyName("MarketCenter")]
        public int? MC { get; set; }
    }
}