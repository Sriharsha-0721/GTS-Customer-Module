using GTS.Application.DTOs;
using GTS.Application.Interfaces.WearerServices;
using Microsoft.AspNetCore.Mvc;

namespace GTS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WearerController : ControllerBase
    {
        private readonly IWearerService _service;

        public WearerController(
            IWearerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetWearer(
            int custId,
            string wearNbr)
        {
            var wearer =
                await _service.GetWearerAsync(
                    custId,
                    wearNbr);

            if (wearer == null)
                return NotFound();

            return Ok(wearer);
        }

        [HttpGet("Next")]
        public async Task<IActionResult> GetNextWearer(
            int custId,
            int? wearerId)
        {
            var wearer =
                await _service.GetNextWearerAsync(
                    custId,
                    wearerId);

            if (wearer == null)
                return NotFound();

            return Ok(wearer);
        }

        [HttpGet("Previous")]
        public async Task<IActionResult> GetPreviousWearer(
            int custId,
            int? wearerId)
        {
            var wearer =
                await _service.GetPreviousWearerAsync(
                    custId,
                    wearerId);

            if (wearer == null)
                return NotFound();

            return Ok(wearer);
        }

        [HttpPost]
        public async Task<IActionResult> SaveWearer(
            UpdateWearerDTO dto)
        {
            var result =
                await _service.SaveWearerAsync(dto);

            if (result < 0)
                return BadRequest();

            return Ok(result);
        }
    }
}