using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WashController : ControllerBase
    {
        private readonly IWashService _service;

        public WashController(IWashService service)
        {
            _service = service;
        }

        [HttpGet("{custId}")]
        public async Task<IActionResult> GetWashDetails(int custId)
        {
            var result =
                await _service.GetWashDetails(custId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("GarmentTypes")]
        public async Task<IActionResult> GetGarmentTypes()
        {
            var result =
                await _service.GetGarmentTypes();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveWash(WashDTO dto)
        {
            var result = await _service.SaveWash(dto);

            return Ok(result);
        }
    }
}