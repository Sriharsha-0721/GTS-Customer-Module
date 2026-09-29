using System.Net.Http.Json;
using GTS.Application.DTOs;
using GTS.Application.Interfaces;

namespace GTS.MVC.Gateways;

public class SoilStationGateway : ISoilStationGateway
{
    private readonly HttpClient _client;

    public SoilStationGateway()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7161/")
        };

        _client.DefaultRequestHeaders.Add(
            "Accept",
            "application/json");
    }

    public async Task<ReceiverDto?> ValidateReceiverAsync(
        long receiverId,
        CancellationToken ct = default)
    {
        var response = await _client.GetAsync(
            $"api/SoilStation/Receiver/{receiverId}",
            ct);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ReceiverDto>(
            cancellationToken: ct);
    }

    public async Task<bool> ValidateContainerAsync(
        string containerId,
        CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync(
            "api/SoilStation/ValidateContainer",
            containerId,
            ct);

        return response.IsSuccessStatusCode;
    }

    public async Task<ScanResultDto> ProcessScanAsync(
        SoilStationScanRequest request,
        CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync(
            "api/SoilStation/Scan",
            request,
            ct);

        var result =
            await response.Content.ReadFromJsonAsync<ScanResultDto>(
                cancellationToken: ct);

        return result ?? new ScanResultDto
        {
            Success = false,
            StatusCode = "API_ERROR",
            Message = "No response received from Scan API."
        };
    }

    public async Task<ScanResultDto> UndoScanAsync(
        SoilStationScanRequest request,
        CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync(
            "api/SoilStation/Undo",
            request,
            ct);

        var result =
            await response.Content.ReadFromJsonAsync<ScanResultDto>(
                cancellationToken: ct);

        return result ?? new ScanResultDto
        {
            Success = false,
            StatusCode = "API_ERROR",
            Message = "No response received from Undo API."
        };
    }

    public async Task<SpecialInstructionsDto?> GetSpecialInstructionsAsync(
        int receiverId,
        CancellationToken ct = default)
    {
        var response = await _client.GetAsync(
            $"api/SoilStation/SpecialInstructions/{receiverId}",
            ct);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<SpecialInstructionsDto>(
            cancellationToken: ct);
    }
}