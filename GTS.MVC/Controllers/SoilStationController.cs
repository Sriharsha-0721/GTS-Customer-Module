using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTS.MVC.Controllers;

[Route("SoilStation")]
public class SoilStationController(
    ISoilStationWebService webService) : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("ValidateReceiver")]
    public async Task<IActionResult> ValidateReceiver(
        long receiverId,
        CancellationToken ct)
    {
        try
        {
            var data = await webService.ValidateReceiverAsync(
                receiverId,
                ct);

            if (data == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Receiver not found or invalid."
                });
            }

            return Json(new
            {
                success = true,
                data
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                message = $"API Connection Error: {ex.Message}"
            });
        }
    }

    [HttpGet("ValidateContainer")]
    public async Task<IActionResult> ValidateContainer(
        string containerId,
        CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(containerId))
            {
                return Json(new
                {
                    success = false,
                    message = "Container barcode is required."
                });
            }

            var valid = await webService.ValidateContainerAsync(
                containerId,
                ct);

            if (!valid)
            {
                return Json(new
                {
                    success = false,
                    message = "Container cannot be found. Please try another."
                });
            }

            return Json(new
            {
                success = true,
                message = "Container Valid"
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                message = $"API Connection Error: {ex.Message}"
            });
        }
    }

    [HttpPost("ScanGarment")]
    public async Task<IActionResult> ScanGarment(
        [FromBody] SoilStationScanRequest request,
        CancellationToken ct)
    {
        try
        {
            var result = await webService.ProcessScanAsync(
                request,
                ct);

            return Json(result);
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                message = $"API Connection Error: {ex.Message}"
            });
        }
    }

    [HttpPost("UndoScan")]
    public async Task<IActionResult> UndoScan(
        [FromBody] SoilStationScanRequest request,
        CancellationToken ct)
    {
        try
        {
            var result = await webService.UndoScanAsync(
                request,
                ct);

            return Json(result);
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                message = $"API Connection Error: {ex.Message}"
            });
        }
    }

    [HttpGet("GetSpecialInstructions")]
    public async Task<IActionResult> GetSpecialInstructions(
        int receiverId,
        CancellationToken ct)
    {
        try
        {
            var result =
                await webService.GetSpecialInstructionsAsync(
                    receiverId,
                    ct);

            if (result == null)
            {
                return Json(new
                {
                    success = false,
                    message = "No special instructions available."
                });
            }

            return Json(new
            {
                success = true,
                data = result
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                message = $"API Connection Error: {ex.Message}"
            });
        }
    }
}