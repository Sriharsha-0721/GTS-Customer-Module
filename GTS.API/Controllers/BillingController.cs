using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _service;

        public BillingController(IBillingService service)
        {
            _service = service;
        }

        [HttpGet("{custId}")]
        public async Task<IActionResult> GetBillingCharges(int custId)
        {
            var result = await _service.GetBillingCharges(custId);
            return Ok(result);
        }

        [HttpGet("BillingData")]
        public async Task<IActionResult> GetBillingData()
        {
            var result = await _service.GetBillingData();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBillingCharge(BillingChargeDTO dto)
        {
            var result = await _service.SaveBillingCharge(dto);
            return Ok(result);
        }

        [HttpDelete("{billingChargesId}")]
        public async Task<IActionResult> DeleteBillingCharge(int billingChargesId)
        {
            var result = await _service.DeleteBillingCharge(billingChargesId);
            return Ok(result);
        }
    }
}