using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WashController : ControllerBase
{
    private readonly IWashService _service;

    public WashController(
        IWashService service)
    {
        _service = service;
    }


    // GET /api/wash/garmenttypes
    [HttpGet("garmenttypes")]
    public async Task<IActionResult>
        GetGarmentTypes(
            CancellationToken ct)
    {
        var result =
            await _service
                .GetGarmentTypesAsync(ct);

        return Ok(result);
    }


    // GET /api/wash/formula/569/26238
    [HttpGet("formula/{marketCenter:int}/{custId:int}")]
    public async Task<IActionResult>
        GetFormula(
            int marketCenter,
            int custId,
            CancellationToken ct)
    {
        if (custId <= 0)
        {
            return BadRequest(
                "Invalid customer.");
        }


        var result =
            await _service
                .GetFormulaAsync(
                    marketCenter,
                    custId,
                    ct);


        return Ok(
            new
            {
                formula = result
            }
        );
    }


    // POST /api/wash/save-formula
    [HttpPost("save-formula")]
    public async Task<IActionResult>
        SaveFormula(
            [FromBody]
            SaveFormulaDto dto,
            CancellationToken ct)
    {
        if (dto == null)
        {
            return BadRequest(
                "Formula data required.");
        }


        if (dto.CustId <= 0)
        {
            return BadRequest(
                "Invalid customer.");
        }


        var result =
            await _service
                .SaveFormulaAsync(
                    dto,
                    ct);


        if (result <= 0)
        {
            return BadRequest(
                "Formula was not saved.");
        }


        return Ok(result);
    }
}