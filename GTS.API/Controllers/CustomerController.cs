using Microsoft.AspNetCore.Mvc;
using GTS.Application.DTOs;
using GTS.Application.Interfaces.CustomerServices;

namespace GTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerDTO>> Get()
        {
            return await _service.GetAllCustomersAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> Get(int id)
        {
            var customer = await _service.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpGet("{id}/edit")]
        public async Task<ActionResult<UpdateCustomerDTO>> GetForUpdate(int id)
        {
            var customer = await _service.GetCustomerForUpdateAsync(id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCustomerDTO customerDto)
        {
            var customer = await _service.AddCustomerAsync(customerDto);
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            [FromBody] UpdateCustomerDTO customerDto)
        {
            if (id != customerDto.CustId)
                return BadRequest();

            await _service.UpdateCustomerAsync(customerDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}