using System.Net.Http.Json;
using GTS.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace GTS.MVC.Controllers
{
    public class WearerController : Controller
    {
        private readonly HttpClient _http;

        public WearerController()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7161/")
            };
        }

        // Main entry / partial loader
        public async Task<IActionResult> Index(int custId, string? wearNbr = null)
        {
            WearerViewModel model = new() { CustId = custId };

            if (!string.IsNullOrWhiteSpace(wearNbr))
            {
                model = await _http.GetFromJsonAsync<WearerViewModel>(
                    $"api/Wearer?custId={custId}&wearNbr={wearNbr}") ?? new();

                model.CustId = custId;
            }

            return Request.Headers["X-Requested-With"] == "XMLHttpRequest"
                ? PartialView("Index", model)
                : View(model);
        }

        // =====================================================
        // WEARER SEARCH MODAL ACTION (Loads the List Partial)
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> SearchModal(int custId)
        {
            var wearers = await _http.GetFromJsonAsync<List<WearerViewModel>>(
                $"api/Wearer/List?custId={custId}") ?? new List<WearerViewModel>();

            // Returns your _WearerSelection.cshtml partial view
            return PartialView("_WearerSelection", wearers);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] WearerViewModel model)
        {
            var dto = new
            {
                WearerId = model.WearerId,
                Locker = model.Locker,
                LockRm = model.LockRm,
                Sex = model.Sex
            };

            var response = await _http.PostAsJsonAsync("api/Wearer", dto);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Unable to save wearer details.");
            }

            return Ok(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Next(int custId, int wearerId)
        {
            var model = await _http.GetFromJsonAsync<WearerViewModel>(
                $"api/Wearer/Next?custId={custId}&wearerId={wearerId}") ?? new();

            return PartialView("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> Previous(int custId, int wearerId)
        {
            var model = await _http.GetFromJsonAsync<WearerViewModel>(
                $"api/Wearer/Previous?custId={custId}&wearerId={wearerId}") ?? new();

            return PartialView("Index", model);
        }
    }
}