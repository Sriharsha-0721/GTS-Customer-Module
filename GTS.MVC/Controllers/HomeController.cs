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

        // GET: /Customer or /Customer/Index
        public async Task<IActionResult> Index(
            int? searchId,
            string? searchName,
            int? route,
            int marketCenter = 569,
            bool showAll = false,
            string? module = null,
            CancellationToken ct = default)
        {
            ViewBag.ActiveTab = "Customer";
            ViewBag.Module = module;
            ViewBag.SearchId = searchId;
            ViewBag.SearchName = searchName;
            ViewBag.MarketCenter = marketCenter;

            // 1. Initial page load: return empty view immediately (displays "Use Search or click Show All")
            if (!showAll && !searchId.HasValue && string.IsNullOrWhiteSpace(searchName) && !route.HasValue)
            {
                return View(new List<CustomersSelListDto>());
            }

            // 2. Only execute SP when user clicked 'Show All' or supplied filter criteria
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

        // POST / GET: /Customer/Search (Used for modal/AJAX dynamic lookup)
        [HttpPost]
        public async Task<IActionResult> Search([FromBody] CustomerFilterRequestDto filter, CancellationToken ct = default)
        {
            if (filter == null)
            {
                filter = new CustomerFilterRequestDto { MarketCenter = 569, NumRecsToFetch = 50 };
            }

            if (filter.MarketCenter == 0)
            {
                filter.MarketCenter = 569;
            }

            var customers = (await _service.GetCustomersBasedOnFilter(filter, ct)).ToList();

            if (!customers.Any())
            {
                ViewBag.Error = "No customers found matching your criteria.";
            }

            return PartialView("_Search", customers);
        }

        // Submodule Handlers (Using CustId from selection grid)
        [HttpGet]
        public async Task<IActionResult> Flags(int custId, CancellationToken ct = default)
        {
            var flags = await _service.GetCustomerFlags(custId, ct);
            if (flags == null) return NotFound();
            return View(flags);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveFlags(int custId, bool ossFlag, bool stfFlag, CancellationToken ct = default)
        {
            await _service.SaveCustomerFlags(custId, ossFlag, stfFlag, ct);
            return RedirectToAction(nameof(Flags), new { custId });
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int custId, CancellationToken ct = default)
        {
            var profile = await _service.GetCustomerProfile(custId, ct);
            if (profile == null) return NotFound();
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateCustomerProfileDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return View("Profile", dto);

            await _service.UpdateCustomerProfile(dto, ct);
            return RedirectToAction(nameof(Profile), new { custId = dto.CustId });
        }
    }
}