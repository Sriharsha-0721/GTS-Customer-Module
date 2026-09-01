using Microsoft.AspNetCore.Mvc;
using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;


namespace GTSCustomerAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DosageController : ControllerBase
{
    private readonly IDosageService _svc;

    public DosageController(IDosageService svc)
    {
        _svc = svc;
    }

    // GET: api/dosage/{custId}
    [HttpGet("{custId:int}")]
    public async Task<IActionResult> Get(
        int custId, CancellationToken ct)
    {
        var result = await _svc
            .GetDosageAsync(custId, ct);
        return Ok(result ?? new CustomerDosageDto
        {
            CustId = custId,
            Dosage = null
        });
    }

    // POST: api/dosage/update
    [HttpPost("update")]
    public async Task<IActionResult> Update(
        [FromBody] CustomerDosageDto dto,
        CancellationToken ct)
    {
        await _svc.UpdateDosageAsync(
            dto.CustId, dto.Dosage, ct);
        return Ok(new { message = "Dosage updated." });
    }
}