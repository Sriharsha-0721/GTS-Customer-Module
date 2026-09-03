using GTS.Application.DTOs;
using GTS.Application.Interfaces.CustomerServices;
using Microsoft.AspNetCore.Mvc;

namespace GTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // Selection Grid (Replaces GetAll/GetById for search)
        [HttpPost("customers")]
        public async Task<IActionResult> GetCustomersBasedOnFilter([FromBody] CustomerFilterRequestDto filterDto, CancellationToken ct = default)
        {
            var customers = await _customerService.GetCustomersBasedOnFilter(filterDto, ct);
            return Ok(customers);
        }

        // Customer Flags Submodule
        [HttpGet("customer-flagdetails/{custId:int}")]
        public async Task<IActionResult> GetCustomerFlags(int custId, CancellationToken ct = default)
        {
            var data = await _customerService.GetCustomerFlags(custId, ct);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost("update-flagdetails/{custId:int}/{ossflag:bool}/{stfflag:bool}")]
        public async Task<IActionResult> SaveCustomerFlags(int custId, bool ossflag, bool stfflag, CancellationToken ct = default)
        {
            var result = await _customerService.SaveCustomerFlags(custId, ossflag, stfflag, ct);
            return Ok(result);
        }

        // Customer Profile Submodule
        [HttpGet("customer-profile/{custId:int}")]
        public async Task<IActionResult> GetCustomerProfile(int custId, CancellationToken ct = default)
        {
            var profile = await _customerService.GetCustomerProfile(custId, ct);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateCustomerProfile([FromBody] UpdateCustomerProfileDto updateDto, CancellationToken ct = default)
        {
            var result = await _customerService.UpdateCustomerProfile(updateDto, ct);
            return Ok(result);
        }
    }
}