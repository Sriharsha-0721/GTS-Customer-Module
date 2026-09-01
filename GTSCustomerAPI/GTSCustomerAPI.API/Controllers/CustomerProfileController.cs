using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerProfileController : ControllerBase
{
    private readonly ICustomerProfileService _svc;

    public CustomerProfileController(
        ICustomerProfileService svc)
    {
        _svc = svc;
    }

    // GET: api/customerprofile/{custId}
    [HttpGet("{custId:int}")]
    public async Task<ActionResult<CustomerProfileDto>>
        Get(int custId)
    {
        var result =
            await _svc.GetByCustIdAsync(custId);
        if (result == null)
            return NotFound(
                $"No profile found for CustId {custId}");
        return Ok(result);
    }

    // POST: api/customerprofile
    [HttpPost]
    public async Task<IActionResult> Save(
        [FromBody] UpdateCustomerProfileDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await _svc.SaveAsync(dto);

        return success
            ? Ok(new { message = "Profile saved." })
            : StatusCode(500, "Save failed.");
    }
}