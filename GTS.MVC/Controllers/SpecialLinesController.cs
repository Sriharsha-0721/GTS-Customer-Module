using GTS.MVC.Models.SpecialLines;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace GTS.MVC.Controllers
{
    public class SpecialLinesController : Controller
    {
        private readonly HttpClient _http;

        public SpecialLinesController()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7161/")
            };
        }

        [HttpGet]
        public async Task<IActionResult> Index(int custId = 0)
        {
            List<SpecialLineViewModel> model = new();

            if (custId > 0)
            {
                model = await _http.GetFromJsonAsync<List<SpecialLineViewModel>>(
                    $"api/SpecialLines/{custId}") ?? new();

                foreach (var item in model)
                {
                    item.IsSelected = item.CustId > 0;
                }
            }

            return PartialView("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int custId, int line)
        {
            await _http.PostAsync($"api/SpecialLines/{custId}/{line}", null);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int line)
        {
            await _http.DeleteAsync($"api/SpecialLines/{line}");
            return Ok();
        }
    }
}