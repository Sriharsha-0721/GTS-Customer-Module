namespace GTS.MVC.Models.SpecialLines
{
    public class SaveSpecialLinesViewModel
    {
        public int CustId { get; set; }

        public List<int> Lines { get; set; } = new();
    }
}