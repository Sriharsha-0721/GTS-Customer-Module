namespace GTS.MVC.Models.Packout
{
    public class PackoutRestrictDataViewModel
    {
        public int CustomerId { get; set; }

        public string PackoutRestrict { get; set; } = "";

        public string ItemType { get; set; } = "";

        public string Color { get; set; } = "";

        public string Size { get; set; } = "";

        public int UpdatedUser { get; set; }
    }
}