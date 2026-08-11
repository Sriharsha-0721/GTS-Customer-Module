using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackoutController : ControllerBase
    {
        private readonly IPackoutService _service;

        public PackoutController(IPackoutService service)
        {
            _service = service;
        }

        [HttpGet("{custId}")]
        public async Task<IActionResult> GetPackoutDetails(int custId)
        {
            var result = await _service.GetPackoutDetails(custId);
            return Ok(result);
        }

        [HttpGet("Items/{custId}")]
        public async Task<IActionResult> GetPackoutItems(int custId)
        {
            var result = await _service.GetPackoutItems(custId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SavePackout(PackoutDTO dto)
        {
            var result = await _service.SavePackout(dto);
            return Ok(result);
        }

        [HttpDelete("{pkoutRestrictId}")]
        public async Task<IActionResult> DeletePackout(int pkoutRestrictId)
        {
            var result = await _service.DeletePackout(pkoutRestrictId);
            return Ok(result);
        }
    }
}