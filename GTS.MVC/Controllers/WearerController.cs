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
            _http = new HttpClient();

            _http.BaseAddress =
                new Uri("https://localhost:7161/");
        }

        public async Task<IActionResult> Index(
            int custId,
            string? wearNbr = null)
        {
            WearerViewModel model = new();

            model.CustId = custId;

            if (!string.IsNullOrWhiteSpace(wearNbr))
            {
                model =
                    await _http.GetFromJsonAsync<WearerViewModel>(
                        $"api/Wearer?custId={custId}&wearNbr={wearNbr}")
                    ?? new();

                model.CustId = custId;
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Save(WearerViewModel model)
        {
            var dto = new
            {
                WearerId = model.WearerId,
                Locker = model.Locker,
                LockRm = model.LockRm,
                Sex = model.Sex
            };

            await _http.PostAsJsonAsync(
                "api/Wearer",
                dto);

            return RedirectToAction(nameof(Index),
                new
                {
                    custId = model.CustId,
                    wearNbr = model.WearNbr
                });
        }
        public async Task<IActionResult> Next(
    int custId,
    int wearerId)
        {
            var model =
                await _http.GetFromJsonAsync<WearerViewModel>(
                    $"api/Wearer/Next?custId={custId}&wearerId={wearerId}")
                ?? new();

            return PartialView("Index", model);
        }
        public async Task<IActionResult> Previous(
    int custId,
    int wearerId)
        {
            var model =
                await _http.GetFromJsonAsync<WearerViewModel>(
                    $"api/Wearer/Previous?custId={custId}&wearerId={wearerId}")
                ?? new();

            return PartialView("Index", model);
        }
    }
}