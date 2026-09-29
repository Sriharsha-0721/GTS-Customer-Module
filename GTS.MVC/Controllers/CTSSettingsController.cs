using System.Net.Http.Json;
using GTS.MVC.Models.CTSSettings;
using Microsoft.AspNetCore.Mvc;

namespace GTS.MVC.Controllers
{
    public class CTSSettingsController : Controller
    {
        private readonly HttpClient _http;

        public CTSSettingsController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("GtsApiClient");
        }

        // GET: /CTSSettings?custId=123
        [HttpGet]
        public async Task<IActionResult> Index(int custId, CancellationToken ct = default)
        {
            if (custId <= 0) return BadRequest("Invalid Customer ID.");

            // 1. Fetch customer details from API
            var custResp = await _http.GetAsync($"api/Customer/customer-profile/{custId}", ct);
            if (!custResp.IsSuccessStatusCode) return NotFound("Customer profile not found.");

            var customer = await custResp.Content.ReadFromJsonAsync<dynamic>(cancellationToken: ct);
            int custNbr = (int)customer.custNbr;
            short mc = (short)customer.marketCenter;

            // 2. Fetch CTS Settings
            var ctsResp = await _http.GetAsync($"api/CTSSetting/{custNbr}", ct);
            CTSSettingsViewModel model;

            if (ctsResp.IsSuccessStatusCode)
            {
                model = await ctsResp.Content.ReadFromJsonAsync<CTSSettingsViewModel>(cancellationToken: ct)
                        ?? new CTSSettingsViewModel();
            }
            else
            {
                model = new CTSSettingsViewModel();
            }

            model.MC = mc;
            model.CustNo = custNbr;

            return PartialView("~/Views/CustomerProfile/CTSSettings.cshtml", model);
        }

        [HttpPost]
        [Route("CTSSettings/Create")]
        public async Task<IActionResult> Create([FromBody] CTSSettingsViewModel model, CancellationToken ct = default)
        {
            if (model == null) return BadRequest("Invalid payload.");

            var payload = new
            {
                MarketCenter = (short)model.MC,
                MC = (short)model.MC,
                CustNbr = model.CustNo,
                CustNo = model.CustNo,
                PrintIssueStatusFlag = (short)(model.PrintIssueStatusFlag ? 1 : 0),
                PrintBornonDateFlag = (short)(model.PrintBornonDateFlag ? 1 : 0),
                NOGFlag = (short)(model.NOGFlag ? 1 : 0),
                LabelHeader = model.LabelHeader ?? string.Empty
            };

            var response = await _http.PostAsJsonAsync("api/CTSSetting", payload, ct);

            if (response.IsSuccessStatusCode)
            {
                return Ok(1);
            }

            var err = await response.Content.ReadAsStringAsync(ct);
            return BadRequest(err);
        }
    }
}