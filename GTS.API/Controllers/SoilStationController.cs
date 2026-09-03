using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoilStationController(ISoilStationService service) : ControllerBase
    {
        [HttpGet("Receiver/{receiverNo}")]
        public async Task<IActionResult> CheckReceiver(long receiverNo)
        {
            var result = await service.CheckReceiverAsync(receiverNo);
            if (result == null || !result.IsSuccess)
            {
                return NotFound(new { success = false, message = "Receiver not found." });
            }

            return Ok(result);
        }

        [HttpPost("ValidateContainer")]
        public async Task<IActionResult> ValidateContainer([FromBody] string containerNo)
        {
            if (string.IsNullOrWhiteSpace(containerNo))
            {
                return BadRequest(new { success = false, message = "Container barcode is required." });
            }

            var isValid = await service.ValidateContainerAsync(containerNo);
            if (!isValid)
            {
                return BadRequest(new { success = false, message = "Container cannot be found. Please try another." });
            }

            return Ok(new { success = true, message = "Container validated successfully." });
        }

        [HttpPost("Scan")]
        public async Task<IActionResult> ProcessScan([FromBody] SoilStationScanRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.GarmentBarcode))
            {
                return BadRequest(new { success = false, message = "Invalid garment scan payload." });
            }

            var result = await service.ProcessScanAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Undo")]
        public async Task<IActionResult> UndoScan([FromBody] SoilStationScanRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.GarmentBarcode))
            {
                return BadRequest(new { success = false, message = "Garment barcode is required for reversal." });
            }

            var success = await service.UndoScanAsync(request.Receiver, request.Barcode, request.UserId);
            if (!success)
            {
                return BadRequest(new { success = false, message = "Garment record is not found or could not be undone." });
            }

            return Ok(new { success = true, message = "Garment scan reversed successfully." });
        }

        [HttpGet("SpecialInstructions/{receiverNo}")]
        public async Task<IActionResult> GetSpecialInstructions(int receiverNo)
        {
            var instructions = await service.GetSpecialInstructionsAsync(receiverNo);
            if (instructions == null)
            {
                return NotFound(new { success = false, message = "No special instructions found for this receiver." });
            }

            return Ok(new { success = true, data = instructions });
        }
    }
}