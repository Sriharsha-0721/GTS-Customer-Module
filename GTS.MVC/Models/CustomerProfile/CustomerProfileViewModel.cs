namespace GTS.MVC.Models.CustomerProfile
{
    public class CustomerProfileViewModel
    {
        public int MC { get; set; }

        public int CustID { get; set; }

        public int CustNo { get; set; }

        public string Customer { get; set; } = "";

        public string Route { get; set; } = "";

        public string GID { get; set; } = "";
    }
}