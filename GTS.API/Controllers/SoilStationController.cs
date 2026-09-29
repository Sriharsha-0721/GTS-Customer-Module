using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SoilStationController : ControllerBase
{
    private readonly ISoilStationService _service;

    public SoilStationController(ISoilStationService service)
    {
        _service = service;
    }

    [HttpGet("Receiver/{receiverId}")]
    public async Task<IActionResult> GetReceiver(long receiverId)
    {
        var receiver = await _service.ValidateReceiverAsync(receiverId);

        if (receiver == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Receiver not found."
            });
        }

        return Ok(receiver);
    }

    [HttpPost("ValidateContainer")]
    public async Task<IActionResult> ValidateContainer(
        [FromBody] string containerId)
    {
        var valid = await _service.ValidateContainerAsync(containerId);

        if (!valid)
        {
            return NotFound(new
            {
                success = false,
                message = "Container not found."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Container valid."
        });
    }

    [HttpPost("Scan")]
    public async Task<IActionResult> Scan(
        [FromBody] SoilStationScanRequest request)
    {
        var result = await _service.ProcessScanAsync(request);

        return Ok(result);
    }

    [HttpPost("Undo")]
    public async Task<IActionResult> Undo(
        [FromBody] SoilStationScanRequest request)
    {
        var result = await _service.UndoScanAsync(
            request.GarmentBarcode,
            request.UserId);

        return Ok(new
        {
            success = result,
            message = result
                ? "Scan undone successfully."
                : "Unable to undo scan."
        });
    }

    [HttpGet("SpecialInstructions/{receiverId}")]
    public async Task<IActionResult> GetSpecialInstructions(
        int receiverId)
    {
        var result =
            await _service.GetSpecialInstructionsAsync(receiverId);

        if (result == null)
        {
            return NotFound(new
            {
                success = false,
                message = "No special instructions available."
            });
        }

        return Ok(result);
    }
}