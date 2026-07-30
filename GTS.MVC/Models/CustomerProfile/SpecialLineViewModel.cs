namespace GTS.MVC.Models.SpecialLines
{
    public class SpecialLineViewModel
    {
        public int? CustId { get; set; }

        public int Line { get; set; }     

        public string ItemCode { get; set; } = "";

        public string Size { get; set; } = "";

        public string Descr { get; set; } = "";

        public bool IsSelected { get; set; }
    }
}