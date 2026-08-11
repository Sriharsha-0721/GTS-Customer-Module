namespace GTS.MVC.Models.Wash
{
    public class WashViewModel
    {
        public int CustId { get; set; }

        public string WashCom { get; set; } = "";

        public List<string> GarmentTypes { get; set; } = new();

        public List<WashFormulaViewModel> Formulas { get; set; } = new();
    }
}