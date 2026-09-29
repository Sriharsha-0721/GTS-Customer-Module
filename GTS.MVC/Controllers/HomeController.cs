using GTS.Application.CustomerServices;
using GTS.Application.DTOs;
using GTS.Application.Interfaces.CustomerServices;
using Microsoft.AspNetCore.Mvc;

namespace GTS.MVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        // Full Page Search: GET /Customer or /Customer/Index
        [HttpGet]
        public async Task<IActionResult> Index(
            int? searchId,
            string? searchName,
            int? route,
            int marketCenter = 0,
            bool showAll = false,
            string? module = null,
            CancellationToken ct = default)
        {
            ViewBag.ActiveTab = "Customer";
            ViewBag.Module = module;
            ViewBag.SearchId = searchId;
            ViewBag.SearchName = searchName;
            ViewBag.MarketCenter = marketCenter;

            // Default initial load: render an empty grid until a filter is applied or "Show All" is clicked
            if (!showAll && !searchId.HasValue && string.IsNullOrWhiteSpace(searchName) && !route.HasValue)
            {
                return View(new List<CustomersSelListDto>());
            }

            var filter = new CustomerFilterRequestDto
            {
                NumRecsToFetch = showAll ? 0 : 50,
                MarketCenter = marketCenter,
                CustNbr = searchId ?? 0,
                CustName = searchName?.Trim() ?? string.Empty,
                Route = route ?? 0,
                WDay = 0,
                GID = string.Empty,
                WearerNbr = 0
            };

            var customers = (await _service.GetCustomersBasedOnFilter(filter, ct)).ToList();

            if (!customers.Any())
            {
                ViewBag.ErrorMessage = "No customers found matching your search.";
            }

            return View(customers);
        }

        // AJAX / Modal Lookup: GET or POST /Customer/Search
        [HttpGet]
        public async Task<IActionResult> Search(
            int? searchId,
            string? searchName,
            int? route,
            int? marketCenter,
            bool showAll = false,
            CancellationToken ct = default)
        {
            // Default to 561 only if user left MC blank
            int mcToUse = (marketCenter.HasValue && marketCenter.Value > 0) ? marketCenter.Value : 561;

            var filter = new CustomerFilterRequestDto
            {
                NumRecsToFetch = showAll ? 500 : 50,
                MarketCenter = mcToUse,
                CustNbr = searchId ?? 0,
                CustName = searchName?.Trim() ?? string.Empty,
                Route = route ?? 0,
                WDay = 0,
                GID = string.Empty,
                WearerNbr = 0
            };

            var customers = (await _service.GetCustomersBasedOnFilter(filter, ct)).ToList();

            if (customers.Count == 0)
            {
                ViewBag.Error = $"No customers found for Market Center {mcToUse}.";
            }

            return PartialView("~/Views/Customer/_Search.cshtml", customers);
        }

        // GET: /Customer/Flags?custId=123
        [HttpGet]
        public async Task<IActionResult> Flags(int custId = 0, CancellationToken ct = default)
        {
            ViewBag.ActiveTab = "Flags";

            if (custId <= 0)
            {
                return View(new CustomerFlagsDto());
            }

            var flags = await _service.GetCustomerFlags(custId, ct)
                        ?? new CustomerFlagsDto { CustId = custId };

            return View(flags);
        }

        // POST: /Customer/SaveFlags
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveFlags(int custId, bool ossFlag, bool stfFlag, CancellationToken ct = default)
        {
            await _service.SaveCustomerFlags(custId, ossFlag, stfFlag, ct);
            return RedirectToAction(nameof(Flags), new { custId });
        }

        // GET: /Customer/Profile?custId=123
        [HttpGet]
        public async Task<IActionResult> Profile(int custId = 0, CancellationToken ct = default)
        {
            ViewBag.ActiveTab = "Profile";

            if (custId <= 0)
            {
                return View(new CustomerProfileDto());
            }

            var profile = await _service.GetCustomerProfile(custId, ct)
                          ?? new CustomerProfileDto { CustId = custId };

            return View(profile);
        }

        // POST: /Customer/UpdateProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateCustomerProfileDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", dto);
            }

            await _service.UpdateCustomerProfile(dto, ct);
            return RedirectToAction(nameof(Profile), new { custId = dto.CustId });
        }
    }
}