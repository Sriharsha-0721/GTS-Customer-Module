using GTS.Application.DTOs;

namespace GTS.Application.Interfaces;

public interface ISoilStationGateway
{
    Task<ReceiverDto?> ValidateReceiverAsync(
        long receiverId,
        CancellationToken ct = default);

    Task<bool> ValidateContainerAsync(
        string containerId,
        CancellationToken ct = default);

    Task<ScanResultDto> ProcessScanAsync(
        SoilStationScanRequest request,
        CancellationToken ct = default);

    Task<ScanResultDto> UndoScanAsync(
        SoilStationScanRequest request,
        CancellationToken ct = default);

    Task<SpecialInstructionsDto?> GetSpecialInstructionsAsync(
        int receiverId,
        CancellationToken ct = default);
}