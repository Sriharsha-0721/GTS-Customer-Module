using System.Net.Http.Json;
using GTS.MVC.Models.MaximumWash;
using Microsoft.AspNetCore.Mvc;

namespace GTS.MVC.Controllers
{
    public class MaximumWashController : Controller
    {
        private readonly HttpClient _http;

        public MaximumWashController()
        {
            _http = new HttpClient();

            _http.BaseAddress =
                new Uri("https://localhost:7161/");
        }

        // GET : MaximumWash
        public async Task<IActionResult> Index(int custId = 0)
        {
            List<MaximumWashViewModel> model = new();

            if (custId > 0)
            {
                model =
                    await _http.GetFromJsonAsync<List<MaximumWashViewModel>>
                    ($"api/MaxWash/{custId}")
                    ?? new();
            }

            return View(model);
        }

        // GET : MaximumWash/Create
        public IActionResult Create(int custId = 0)
        {
            MaximumWashViewModel model = new();

            model.CustId = custId;

            return View(model);
        }

        // POST : MaximumWash/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaximumWashViewModel model)
        {
            var dto = new
            {
                CustId = model.CustId,
                ItemCode = model.ItemCode,
                MaxWash = model.MaxWash,
                MaxWeeks = model.MaxWeeks,
                MaxCycles = model.MaxCycles
            };

            var response = await _http.PostAsJsonAsync(
                "api/MaxWash",
                dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                TempData["Error"] = error;

                return View(model);
            }

            TempData["Success"] = "Maximum Wash added successfully.";

            return RedirectToAction(nameof(Index),
                new
                {
                    custId = model.CustId
                });
        }
    }
}