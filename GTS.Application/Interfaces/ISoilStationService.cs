using GTS.Application.DTOs;
using System.Threading.Tasks;

namespace GTS.Application.Interfaces
{
    public interface ISoilStationService
    {
        Task<ReceiverDto?> ValidateReceiverAsync(long receiverId);
        Task<ReceiverDto?> CheckReceiverAsync(long receiverId);
        Task<bool> ValidateContainerAsync(string containerId);
        Task<ScanResultDto> ProcessScanAsync(SoilStationScanRequest request);
        Task<ScanResultDto> ProcessGarmentScanAsync(ScanGarmentRequestDto request);
        Task<bool> UndoScanAsync(string garmentBarcode, int userId);
        Task<bool> UndoScanAsync(long receiverId, string garmentBarcode, int userId);
        Task<SpecialInstructionsDto?> GetSpecialInstructionsAsync(int receiverId);
    }
}