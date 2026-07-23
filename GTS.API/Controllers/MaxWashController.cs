using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaxWashController : ControllerBase
    {
        private readonly IMaxWashService _service;

        public MaxWashController(IMaxWashService service)
        {
            _service = service;
        }

        [HttpGet("{custId}")]
        public async Task<IActionResult> Get(int custId)
        {
            var data = await _service.GetMaxWash(custId);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MaxWashDTO dto)
        {
            var result = await _service.CreateMaxWash(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MaxWashDTO dto)
        {
            var result = await _service.UpdateMaxWash(dto);
            return Ok(result);
        }

        [HttpDelete("{custId}/{itemCode}")]
        public async Task<IActionResult> Delete(int custId, string itemCode)
        {
            var result = await _service.DeleteMaxWash(custId, itemCode);
            return Ok(result);
        }
    }
}