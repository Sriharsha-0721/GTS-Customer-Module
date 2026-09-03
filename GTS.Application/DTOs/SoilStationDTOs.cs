using System;

namespace GTS.Application.DTOs
{
    public class SoilStationScanRequest
    {
        public string GarmentBarcode { get; set; } = string.Empty;
        public string Barcode
        {
            get => GarmentBarcode;
            set => GarmentBarcode = value;
        }

        public long ReceiverId { get; set; }
        public long Receiver
        {
            get => ReceiverId;
            set => ReceiverId = value;
        }

        public string ContainerId { get; set; } = string.Empty;
        public int PickupDay { get; set; }
        public int UserId { get; set; } = 1;
        public bool IsRepair { get; set; } = false;
    }

    public class ScanGarmentRequestDto : SoilStationScanRequest
    {
    }

    public class ReceiverDto
    {
        public bool IsSuccess { get; set; } = true;
        public long ReceiverId { get; set; }
        public long Receiver
        {
            get => ReceiverId;
            set => ReceiverId = value;
        }
        public int CustId { get; set; }
        public int MarketCenter { get; set; }
        public int CustNbr { get; set; }
        public string? CustomerName { get; set; }
        public string? UserName { get; set; }
        public string? Status { get; set; }
        public string? StopDt { get; set; }
        public string? WashComments { get; set; }
        public string? SoilComments { get; set; }
    }

    public class GarmentInfoDto
    {
        public string Garment { get; set; } = string.Empty;
        public int? WearItemId { get; set; }
        public int CustId { get; set; }
        public int? WearerId { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string? Size { get; set; }
        public int ProcStn { get; set; }
        public string? ScanCode { get; set; }
        public DateTime? ScanDt { get; set; }
        public string? ServCode { get; set; }
        public string? StopDt { get; set; }
    }

    public class ScanResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string StatusCode { get; set; } = "OK";
        public string? Garment { get; set; }
        public string? ItemCode { get; set; }
        public string? Size { get; set; }
        public int ScanLogId { get; set; }
    }

    public class SpecialInstructionsDto
    {
        public int ReceiverId { get; set; }
        public string? CustomerName { get; set; }
        public int CustomerNumber { get; set; }
        public string? WashComments { get; set; }
        public string? SoilComments { get; set; }
        public string? ReceivingComments { get; set; }
        public string? ProductionComments { get; set; }
        public string? OfficeComments { get; set; }
        public string? GenOfficeComments { get; set; }
        public string? MerControlComments { get; set; }
        public string? DryerComments { get; set; }
        public string? MainCleanRoomComments { get; set; }
        public string? PackoutComments { get; set; }
        public string? ShippingComments { get; set; }
        public string? DriverComments { get; set; }
        public string? MendComments { get; set; }
        public string? QAComments { get; set; }
        public string? CustServiceComments { get; set; }
        public string? BillingComments { get; set; }
        public string? QAInspComments { get; set; }
    }
}