using GTS.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace GTS.Application.Interfaces
{
    public interface ISoilStationRepository
    {
        Task<ReceiverDto?> GetReceiverDataAsync(long receiverId);
        Task<bool> CheckContainerAsync(string containerId);
        Task<GarmentInfoDto?> GetGarmentInfoAsync(string garmentBarcode);
        Task<bool> CheckPickUpDayAsync(int custId, int pickupDay);
        Task<int> CheckExceedsMaxWashAsync(int custId, string itemCode, string garment);
        Task<bool> UpdateGarminServAsync(string garment, string pScanCode, DateTime? pScanDt, string scanCode, DateTime scanDt, int procStn, int updtUser, DateTime updtTime);
        Task<int> LogScanAsync(string barcode, string operType, string scanCode, int custId, int wearerId, int wearItemId, string itemCode, string size, string servCode, string statFlag, DateTime lotDate, int procStn, int userId, long receiverId, string containerId, int overlayNbr);
        Task<SpecialInstructionsDto?> GetSpecialInstructionsAsync(int receiverId);
    }
}