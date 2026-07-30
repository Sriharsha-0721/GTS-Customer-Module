using GTS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GTS.Application.DTOs;

namespace GTS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialLinesController : ControllerBase
    {
        private readonly ISpecialLinesService _service;

        public SpecialLinesController(ISpecialLinesService service)
        {
            _service = service;
        }

        [HttpGet("{custId}")]
        public async Task<IActionResult> Get(
            int custId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 14)
        {
            var result = await _service.GetSpecialLines(
                custId,
                page,
                pageSize);

            return Ok(result);
        }

        [HttpPost("{custId}/{line}")]
        public async Task<IActionResult> Add(int custId, int line)
        {
            await _service.AddSpecialLine(custId, line);
            return Ok();
        }

        [HttpDelete("{line}")]
        public async Task<IActionResult> Delete(int line)
        {
            await _service.DeleteSpecialLine(line);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Save(
    [FromBody] SaveSpecialLinesRequest request)
        {
            await _service.SaveSpecialLines(request);

            return Ok();
        }
    }
}