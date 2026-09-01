using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerAdminController : ControllerBase
{
    private readonly ICustomerAdminService _service;

    public CustomerAdminController(
        ICustomerAdminService service)
    {
        _service = service;
    }

    // GET: api/customeradmin/{custId}
    [HttpGet("{custId:int}")]
    public async Task<ActionResult<CustomerAdminDto>>
        GetByCustId(int custId)
    {
        var result = await _service.GetByCustIdAsync(custId);
        if (result == null)
            return NotFound(
                $"No data for CustId {custId}");
        return Ok(result);
    }

    // POST: api/customeradmin
    [HttpPost]
    public async Task<IActionResult> Save(
        [FromBody] SaveCustomerAdminDto dto)
    {
        var success = await _service.SaveAsync(dto);
        if (!success)
            return StatusCode(500, "Save failed.");
        return Ok(new { message = "Customer saved." });
    }
}