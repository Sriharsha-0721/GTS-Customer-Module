using System.Net.Http.Json;
using GTS.MVC.Models.CTSSettings;
using Microsoft.AspNetCore.Mvc;

namespace GTS.MVC.Controllers
{
    public class CTSSettingsController : Controller
    {
        private readonly HttpClient _http;

        public CTSSettingsController(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://localhost:7161/");
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CTSSettingsViewModel model)
        {
            var response =
                await _http.PostAsJsonAsync(
                    "api/CTSSetting",
                    new
                    {
                        MarketCenter = model.MC,
                        CustNbr = model.CustNo,
                        PrintIssueStatusFlag = model.PrintIssueStatusFlag ? (short)1 : (short)0,
                        PrintBornonDateFlag = model.PrintBornonDateFlag ? (short)1 : (short)0,
                        NOGFlag = model.NOGFlag ? (short)1 : (short)0,
                        LabelHeader = model.LabelHeader
                    });

            if (response.IsSuccessStatusCode)
                return Ok();

            return BadRequest();
        }
    }
}