using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CTSSettingController : ControllerBase
    {
        private readonly ICTSSettingService _service;

        public CTSSettingController(ICTSSettingService service)
        {
            _service = service;
        }

        [HttpGet("{custNbr}")]
        public async Task<IActionResult> Get(int custNbr)
        {
            var result = (await _service.GetCTSSettingDetails(custNbr)).ToList();

            var first = result.FirstOrDefault();

            if (first != null)
            {
                Console.WriteLine("===== API =====");
                    Console.WriteLine($"Issue = {first.PrintIssueStatusFlag}");
                Console.WriteLine($"Born  = {first.PrintBornonDateFlag}");
                Console.WriteLine($"NOG   = {first.NOGFlag}");
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCTSSettingDTO dto)
        {
            var result = await _service.SaveCTSSettings(dto);
            return Ok(result);
        }
    }
}