using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PackoutController : ControllerBase
{
    private readonly IPackoutService _svc;

    public PackoutController(
        IPackoutService svc)
    {
        _svc = svc;
    }



    // GET /api/packout/26238
    [HttpGet("{custId:int}")]
    public async Task<IActionResult>
        GetPackoutDetails(
            int custId,
            CancellationToken ct)
    {
        if (custId <= 0)
        {
            return BadRequest(
                "Invalid customer.");
        }

        var result =
            await _svc
                .GetPackoutByCustIdAsync(
                    custId,
                    ct);

        return Ok(result);
    }


    // GET /api/packout/Items/26238
    [HttpGet("Items/{custId:int}")]
    public async Task<IActionResult>
        GetPackoutItems(
            int custId,
            CancellationToken ct)
    {
        if (custId <= 0)
        {
            return BadRequest(
                "Invalid customer.");
        }

        var result =
            await _svc
                .GetPackoutItemsAsync(
                    custId,
                    ct);

        return Ok(result);
    }


    // POST /api/packout
    [HttpPost]
    public async Task<IActionResult>
        SavePackout(
            [FromBody]
            PackoutSaveDto dto,
            CancellationToken ct)
    {
        if (dto == null)
        {
            return BadRequest(
                "Packout data required.");
        }

        if (dto.CustId <= 0)
        {
            return BadRequest(
                "Invalid customer.");
        }

        if (string.IsNullOrWhiteSpace(
            dto.Item))
        {
            return BadRequest(
                "Item is required.");
        }

        var result =
            await _svc
                .SavePackoutAsync(
                    dto,
                    ct);

        if (result <= 0)
        {
            return BadRequest(
                "Packout was not saved.");
        }

        return Ok(result);
    }


    // DELETE /api/packout/10
    [HttpDelete("{pkoutRestrictId:int}")]
    public async Task<IActionResult>
        DeletePackout(
            int pkoutRestrictId,
            CancellationToken ct)
    {
        if (pkoutRestrictId <= 0)
        {
            return BadRequest(
                "Invalid Packout Id.");
        }

        var result =
            await _svc
                .DeletePackoutAsync(
                    pkoutRestrictId,
                    ct);

        if (result <= 0)
        {
            return BadRequest(
                "Packout delete failed.");
        }

        return Ok(result);
    }
}