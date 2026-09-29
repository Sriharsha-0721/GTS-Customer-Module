using GTS.Application.DTOs;
using GTS.Application.Interfaces;

namespace GTS.Application.Services;

public class SoilStationWebService(
    ISoilStationGateway gateway) : ISoilStationWebService
{
    public Task<ReceiverDto?> ValidateReceiverAsync(
        long receiverId,
        CancellationToken ct = default)
        => gateway.ValidateReceiverAsync(receiverId, ct);

    public Task<bool> ValidateContainerAsync(
        string containerId,
        CancellationToken ct = default)
        => gateway.ValidateContainerAsync(containerId, ct);

    public Task<ScanResultDto> ProcessScanAsync(
        SoilStationScanRequest request,
        CancellationToken ct = default)
        => gateway.ProcessScanAsync(request, ct);

    public Task<ScanResultDto> UndoScanAsync(
        SoilStationScanRequest request,
        CancellationToken ct = default)
        => gateway.UndoScanAsync(request, ct);

    public Task<SpecialInstructionsDto?> GetSpecialInstructionsAsync(
        int receiverId,
        CancellationToken ct = default)
        => gateway.GetSpecialInstructionsAsync(receiverId, ct);
}