using Microsoft.AspNetCore.Mvc;
using GTS.Application.DTOs;
using GTS.Application.Interfaces.CustomerServices;

namespace GTS.MVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index( 
            int? searchId,
            string? searchName,
            string? searchCity,
            bool showAll = false,
            string? module = null)
        {
            ViewBag.ActiveTab = "Customer";
            ViewBag.Module = module;
            ViewBag.SearchId = searchId;
            ViewBag.SearchName = searchName;
            ViewBag.SearchCity = searchCity;

            // Default — show nothing
            if (!showAll && !searchId.HasValue
                && string.IsNullOrEmpty(searchName)
                && string.IsNullOrEmpty(searchCity))
            {
                return View(new List<CustomerDTO>());
            }

            // Fetch ALL customers from DB via SP
            var allCustomers = await _service.GetAllCustomersAsync();

            // Filter in C# based on what user typed
            var filtered = allCustomers.Where(c =>
                (!searchId.HasValue || c.CustId == searchId.Value)
                &&
                (string.IsNullOrEmpty(searchName) ||
                 (c.Name != null && c.Name.Contains(searchName,
                  StringComparison.OrdinalIgnoreCase)))
                &&
                (string.IsNullOrEmpty(searchCity) ||
                 (c.City != null && c.City.Contains(searchCity,
                  StringComparison.OrdinalIgnoreCase)))
            ).ToList();

            if (!filtered.Any())
                ViewBag.ErrorMessage = "No customers found matching your search.";

            return View(filtered);
        }

        // GET: /Customer/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _service.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        // GET: /Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerDTO customerDto)
        {
            if (!ModelState.IsValid)
                return View(customerDto);

            await _service.AddCustomerAsync(customerDto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Customer/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _service.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();

            var updateDto = new UpdateCustomerDTO
            {
                CustId = customer.CustId,
                MarketCenter = customer.MarketCenter,
                CustNbr = customer.CustNbr,
                Account = customer.Account,
                Dept = customer.Dept,
                Name = customer.Name,
                Route = customer.Route,
                GID = customer.GID,
                StopDt = customer.StopDt,
                Addr1 = customer.Addr1,
                Addr2 = customer.Addr2,
                City = customer.City,
                State = customer.State,
                Zip = customer.Zip,
                Phone = customer.Phone,
                Fax = customer.Fax,
                Freq = customer.Freq,
                Contact = customer.Contact,
                CreateDt = customer.CreateDt,
                PONumber = customer.PONumber,
                BillName = customer.BillName,
                BillAddr = customer.BillAddr,
                BillCity = customer.BillCity,
                BillState = customer.BillState,
                BillZipCod = customer.BillZipCod,
                BillPhone = customer.BillPhone,
                UpdtTime = customer.UpdtTime,
                UpdtUser = 1
            };

            return View(updateDto);
        }

        // POST: /Customer/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCustomerDTO customerDto)
        {
            if (id != customerDto.CustId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(customerDto);

            await _service.UpdateCustomerAsync(customerDto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Customer/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _service.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        // POST: /Customer/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Search(
    int? searchId,
    string? searchName,
    string? searchCity,
    bool showAll = false)
        {
            ViewBag.ActiveTab = "Customer";

            List<CustomerDTO> customers = new();

            if (showAll ||
                searchId.HasValue ||
                !string.IsNullOrEmpty(searchName) ||
                !string.IsNullOrEmpty(searchCity))
            {
                var allCustomers = await _service.GetAllCustomersAsync();

                customers = allCustomers
                    .Where(c =>
                        (!searchId.HasValue || c.CustId == searchId.Value) &&
                        (string.IsNullOrEmpty(searchName) ||
                         (c.Name != null && c.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase))) &&
                        (string.IsNullOrEmpty(searchCity) ||
                         (c.City != null && c.City.Contains(searchCity, StringComparison.OrdinalIgnoreCase))))
                    .ToList();
                if (!customers.Any())
                {
                    if (searchId.HasValue)
                    {
                        ViewBag.Error =
                            "Customer ID not found.";
                    }
                    else if (!string.IsNullOrWhiteSpace(searchName))
                    {
                        ViewBag.Error =
                            "Customer name not found.";
                    }
                    else if (!string.IsNullOrWhiteSpace(searchCity))
                    {
                        ViewBag.Error =
                            "City not found.";
                    }
                }
            }

            return PartialView("_Search", customers);
        }

    }
}