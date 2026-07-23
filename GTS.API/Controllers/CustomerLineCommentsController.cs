using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GTS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerLineCommentsController : ControllerBase
    {
        private readonly
            ICustomerLineCommentsService _service;

        public CustomerLineCommentsController(
            ICustomerLineCommentsService service)
        {
            _service = service;
        }

        [HttpGet("{custId}")]
        public async Task<IActionResult> Get(int custId)
        {
            var data =
                await _service.GetCustWearItems(custId);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Save(
            CustLineCmtsDTO dto)
        {
            var result =
                await _service.InsertCustLineCmts(dto);

            return Ok(result);
        }
    }
}