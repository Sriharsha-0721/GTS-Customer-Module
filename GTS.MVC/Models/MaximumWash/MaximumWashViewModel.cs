namespace GTS.MVC.Models.MaximumWash
{
    public class MaximumWashViewModel
    {
        public int CustId { get; set; }

        public string ItemCode { get; set; } = "";

        public string OldItemCode { get; set; } = string.Empty;

        public string NewItemCode { get; set; } = string.Empty;

        public int MaxWash { get; set; }

        public int MaxWeeks { get; set; }

        public int MaxCycles { get; set; }

        // selected customer info
        public int CustNo { get; set; }

        public string Customer { get; set; } = "";

        public string Route { get; set; } = "";

        public string GID { get; set; } = "";
    }
}