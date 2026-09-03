namespace GTS.MVC.Models.SoilStation
{
    public class ScanGarmentRequestDTO
    {
        public int MarketCenter { get; set; } = 569;
        public string ReceiverNo { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;
        public string GarmentId { get; set; } = string.Empty;
        public string PickUpDay { get; set; } = string.Empty;
        public DateTime PickUpDate { get; set; }
    }

    public class ScanGarmentResultDTO
    {
        public bool Success { get; set; }
        public string Status { get; set; } = string.Empty; // "VALID", "DUPLICATE", "NOT_FOUND", "DAY_LOT_ERROR", "REQUIRED_PULL"
        public string Message { get; set; } = string.Empty;

        // Special Instructions & Comments
        public bool HasSpecialInstructions { get; set; }
        public string? SpecialInstructionsText { get; set; }

        // Live Statistics to refresh the UI boxes
        public int ValidScans { get; set; }
        public int DayOrLotErrors { get; set; }
        public int NotOnFile { get; set; }
        public int RequiredPull { get; set; }
        public int Duplicates { get; set; }
    }
}