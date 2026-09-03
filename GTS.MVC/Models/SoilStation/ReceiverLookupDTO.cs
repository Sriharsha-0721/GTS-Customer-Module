namespace GTS.MVC.Models.SoilStation
{
    public class ReceiverLookupDTO
    {
        public string ReceiverNo { get; set; } = string.Empty;
        public int CustId { get; set; }
        public string CustNo { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime PickUpDate { get; set; } = DateTime.Today;
        public string PickUpDay { get; set; } = string.Empty;
    }
}