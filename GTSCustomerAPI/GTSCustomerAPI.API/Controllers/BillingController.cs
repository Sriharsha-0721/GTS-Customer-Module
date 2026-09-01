using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.API.Controllers;

[ApiController]
[Route("api/billing")]
public class BillingController : ControllerBase
{
    private readonly IBillingService _service;

    public BillingController(
        IBillingService service)
    {
        _service = service;
    }

    // =========================================
    // GET BILLING DROPDOWN
    // GET /api/billing/BillingData
    // =========================================

    [HttpGet("BillingData")]
    public async Task<IActionResult>
        GetBillingData(
            CancellationToken ct)
    {
        var data =
            await _service
                .GetBillingDataAsync(ct);

        return Ok(data);
    }


    // =========================================
    // GET CUSTOMER BILLING CHARGES
    // GET /api/billing/26239
    // =========================================

    [HttpGet("{custId:int}")]
    public async Task<IActionResult>
        GetBillingCharges(
            int custId,
            CancellationToken ct)
    {
        var data =
            await _service
                .GetBillingbyCustIdAsync(
                    custId,
                    ct);

        return Ok(data);
    }


    // =========================================
    // SAVE
    // POST /api/billing
    // =========================================

    [HttpPost]
    public async Task<IActionResult>
        SaveBilling(
            [FromBody] BillingChargeDto dto,
            CancellationToken ct)
    {
        if (dto.CustId <= 0)
            return BadRequest(
                "Invalid customer.");

        if (dto.ChargeTypeId <= 0)
            return BadRequest(
                "Please select Charge Type.");

        if (dto.Charges < 0)
            return BadRequest(
                "Invalid charge amount.");

        var result =
            await _service
                .SaveBillingAsync(
                    dto,
                    ct);

        return Ok(result);
    }


    // =========================================
    // DELETE
    // DELETE /api/billing/10
    // =========================================

    [HttpDelete("{billingChargeId:int}")]
    public async Task<IActionResult>
        DeleteBilling(
            int billingChargeId,
            CancellationToken ct)
    {
        var result =
            await _service
                .DeleteBillingAsync(
                    billingChargeId,
                    ct);

        return Ok(result);
    }
}