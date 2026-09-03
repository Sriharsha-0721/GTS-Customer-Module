namespace GTS.MVC.Models.SoilStation
{
    public class SoilStationViewModel
    {
        public int MarketCenter { get; set; } = 569;
        public string? ReceiverNo { get; set; }
        public string? FindGarmentNo { get; set; }

        public string PickUpDay { get; set; } = DateTime.Today.DayOfWeek.ToString();
        public DateTime PickUpDate { get; set; } = DateTime.Today;

        public int CustId { get; set; }
        public string? CustNo { get; set; }
        public string? CustomerName { get; set; }
        public string? CreatedBy { get; set; }

        public string? ContainerNo { get; set; }
        public string? GarmentId { get; set; }

        // Scanning Statistics Counters
        public int ValidScans { get; set; }
        public int DayOrLotErrors { get; set; }
        public int NotOnFile { get; set; }
        public int RequiredPull { get; set; }
        public int Duplicates { get; set; }
    }
}