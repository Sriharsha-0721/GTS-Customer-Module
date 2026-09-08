using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CTSSettingsController : ControllerBase
{
    private readonly ICTSSettingsService _service;

    public CTSSettingsController(ICTSSettingsService service)
    {
        _service = service;
    }

    // GET: api/ctssettings/{custId}
    [HttpGet("{custId:int}")]
    public async Task<ActionResult<CTSSettingsDto>>
        GetByCustId(int custId)
    {
        var result = await _service.GetByCustIdAsync(custId);
        if (result == null)
            return NotFound(
                $"No CTS Settings for CustId {custId}");
        return Ok(result);
    }

    // POST: api/ctssettings
    [HttpPost]
    public async Task<IActionResult> Save(
        [FromBody] SaveCTSSettingsDto dto)
    {
        var success =
    await _service.SaveAsync(dto);
        if (!success)
            return StatusCode(500, "Save failed.");
        return Ok(new { message = "CTS Settings saved." });
    }
}