using GTSCustomerAPI.Web.Models;
using GTSCustomerAPI.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.Web.Controllers;

public class CustomerController : Controller
{
    private readonly ApiService _api;

    public CustomerController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index(
    int? searchId, string? searchName,
    string? searchCity, string? searchPhone,
    bool showAll = false,
    string? returnTo = null)
    {
        if (searchId.HasValue && searchId <= 0)
            searchId = null;

        // ✅ Auto load all customers when coming from tab
        // (returnTo is set = user came from a tab)
        if (!string.IsNullOrEmpty(returnTo)
            && !searchId.HasValue
            && string.IsNullOrWhiteSpace(searchName)
            && string.IsNullOrWhiteSpace(searchCity)
            && string.IsNullOrWhiteSpace(searchPhone))
        {
            showAll = true;
        }

        List<CustomerViewModel> customers = new();
        bool showTable = false;
        bool hasSearch =
            searchId.HasValue ||
            !string.IsNullOrWhiteSpace(searchName) ||
            !string.IsNullOrWhiteSpace(searchCity) ||
            !string.IsNullOrWhiteSpace(searchPhone);

        if (hasSearch)
        {
            var all = await _api.GetAllAsync();
            customers = all.Where(c =>
                (!searchId.HasValue ||
                  c.CustId == searchId.Value) &&
                (string.IsNullOrWhiteSpace(searchName) ||
                 (c.Name ?? "").Contains(searchName,
                  StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(searchCity) ||
                 (c.City ?? "").Contains(searchCity,
                  StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(searchPhone) ||
                 (c.Phone ?? "").Contains(searchPhone,
                  StringComparison.OrdinalIgnoreCase))
            ).ToList();
            ViewBag.NotFound = !customers.Any();
            showTable = true;
        }
        else if (showAll)
        {
            customers = await _api.GetAllAsync();
            ViewBag.NotFound = false;
            showTable = true;
        }
        else
        {
            customers = new();
            ViewBag.NotFound = false;
            showTable = false;
        }

        ViewBag.SearchId = searchId;
        ViewBag.SearchName = searchName;
        ViewBag.SearchCity = searchCity;
        ViewBag.SearchPhone = searchPhone;
        ViewBag.TotalCount = customers.Count;
        ViewBag.ShowTable = showTable;
        ViewBag.ReturnTo = returnTo;

        ViewBag.SelectedCustId =
    HttpContext.Session.GetInt32("SelectedCustId");

        ViewBag.SelectedCustNbr =
            HttpContext.Session.GetInt32("SelectedCustNbr");

        ViewBag.SelectedName =
            HttpContext.Session.GetString("SelectedName");

        ViewBag.SelectedRoute =
            HttpContext.Session.GetInt32("SelectedRoute");

        ViewBag.SelectedGID =
            HttpContext.Session.GetString("SelectedGID");

        return View(customers);
    }
    // ✅ POST: Select customer + redirect back to caller

    [HttpPost]
    public IActionResult SelectCustomer(
    int custId,
    int custNbr,
    string? name,
    int route,
    string? gid,
    int marketCenter = 0)
    {
        // Store selected customer in Session
        HttpContext.Session.SetInt32(
            "SelectedCustId",
            custId);

        HttpContext.Session.SetInt32(
            "SelectedCustNbr",
            custNbr);

        HttpContext.Session.SetString(
            "SelectedName",
            name ?? "");

        HttpContext.Session.SetInt32(
            "SelectedRoute",
            route);

        HttpContext.Session.SetString(
            "SelectedGID",
            gid ?? "");

        HttpContext.Session.SetInt32(
            "SelectedMC",
            marketCenter);

        // IMPORTANT:
        // Do not automatically open Customer Profile,
        // Wearer, Customer Admin or CTS Settings.
        // Stay on Customer Selection.
        return RedirectToAction(
            nameof(Index),
            new
            {
                showAll = true
            });
    }
    public IActionResult ClearSelection()
    {
        HttpContext.Session.Remove(
            "SelectedCustId");

        HttpContext.Session.Remove(
            "SelectedCustNbr");

        HttpContext.Session.Remove(
            "SelectedName");

        HttpContext.Session.Remove(
            "SelectedRoute");

        HttpContext.Session.Remove(
            "SelectedGID");

        HttpContext.Session.Remove(
            "SelectedMC");

        return RedirectToAction(
            nameof(Index),
            new
            {
                showAll = true
            });
    }

    public IActionResult Create()
    {
        return View(new CreateCustomerViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateCustomerViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var customer = await _api.CreateAsync(model);
        if (customer != null)
        {
            TempData["Success"] =
                $"Customer created with CustId " +
                $"'{customer.CustId}'.";
            return RedirectToAction(
                nameof(Index), new { showAll = true });
        }
        ModelState.AddModelError(
            "", "Failed to create customer.");
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var c = await _api.GetByIdAsync(id);
        if (c == null) return NotFound();
        return View(new UpdateCustomerViewModel
        {
            Account = c.Account,
            Dept = c.Dept,
            Name = c.Name,
            Route = c.Route,
            Addr1 = c.Addr1,
            Addr2 = c.Addr2,
            City = c.City,
            State = c.State,
            Zip = c.Zip,
            Phone = c.Phone,
            Fax = c.Fax,
            Freq = c.Freq,
            OSSFlag = c.OSSFlag,
            PONumber = c.PONumber,
            BillName = c.BillName,
            BillAddr = c.BillAddr,
            BillCity = c.BillCity,
            BillState = c.BillState,
            BillZipCod = c.BillZipCod,
            UpdtUser = c.UpdtUser
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, UpdateCustomerViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var success = await _api.UpdateAsync(id, model);
        if (success)
        {
            TempData["Success"] =
                "Customer updated successfully!";
            return RedirectToAction(
                nameof(Index), new { showAll = true });
        }
        ModelState.AddModelError(
            "", "Failed to update.");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _api.DeleteAsync(id);
        TempData["Success"] = "Customer deleted!";
        return RedirectToAction(
            nameof(Index), new { showAll = true });
    }
}