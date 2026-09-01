using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WearerController : ControllerBase
{
    private readonly IWearerService _svc;

    public WearerController(IWearerService svc)
    {
        _svc = svc;
    }

    // GET api/wearer/{custId}/all
    [HttpGet("{custId:int}/all")]
    public async Task<IActionResult> GetAll(
        int custId, CancellationToken ct)
    {
        var r = await _svc.GetAllWearers(custId, ct);
        return Ok(r);
    }

    // GET api/wearer/{custId}/first
    [HttpGet("{custId:int}/first")]
    public async Task<IActionResult> GetFirst(
        int custId, CancellationToken ct)
    {
        var r = await _svc.NextWearerDetails(
            custId, null, ct);
        return Ok(r);
    }

    // GET api/wearer/{custId}/next/{lastWrId}
    [HttpGet("{custId:int}/next/{lastWrId:int}")]
    public async Task<IActionResult> GetNext(
        int custId, int lastWrId, CancellationToken ct)
    {
        var r = await _svc.NextWearerDetails(
            custId, lastWrId, ct);
        return Ok(r);
    }

    // GET api/wearer/{custId}/prev/{firstWrId}
    [HttpGet("{custId:int}/prev/{firstWrId:int}")]
    public async Task<IActionResult> GetPrev(
        int custId, int firstWrId, CancellationToken ct)
    {
        var r = await _svc.PrevWearerDetails(
            custId, firstWrId, ct);
        return Ok(r);
    }

    // GET api/wearer/{custId}/search/{wearNbr}
    [HttpGet("{custId:int}/search/{wearNbr}")]
    public async Task<IActionResult> GetByNbr(
        int custId, string wearNbr, CancellationToken ct)
    {
        var r = await _svc.GetWearerDetails(
            custId, wearNbr, ct);
        return Ok(r);
    }

    // POST api/wearer
    [HttpPost]
    public async Task<IActionResult> Save(
        [FromBody] WearerDto dto, CancellationToken ct)
    {
        var rows = await _svc.SaveWearer(dto, ct);
        if (rows < 0)
            return StatusCode(500, "Save failed.");
        return Ok(new { message = "Saved.", rows });
    }
}