using GTSCustomerAPI.Web.Models;
using GTSCustomerAPI.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.Web.Controllers;

public class WearerController : Controller
{
    private readonly ApiService _api;

    public WearerController(ApiService api)
    {
        _api = api;
    }

    private WearerViewModel FromSession() => new()
    {
        CustId   = HttpContext.Session
            .GetInt32("SelectedCustId")  ?? 0,
        CustNbr  = HttpContext.Session
            .GetInt32("SelectedCustNbr") ?? 0,
        CustName = HttpContext.Session
            .GetString("SelectedName"),
        Route    = HttpContext.Session
            .GetInt32("SelectedRoute")   ?? 0,
        GID      = HttpContext.Session
            .GetString("SelectedGID")
    };

    private static WearerViewModel Fill(
        WearerViewModel m, WearerViewModel? w)
    {
        if (w == null) return m;
        m.WearerId  = w.WearerId;
        m.WearNbr   = w.WearNbr;
        m.FirstName = w.FirstName;
        m.LastName  = w.LastName;
        m.SexInt    = w.SexInt;
        m.Locker    = w.Locker;
        m.LockRm    = w.LockRm;
        return m;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = FromSession();

        if (model.CustId == 0)
        {
            // No customer — show customer list
            // with Wearer tab active
            return RedirectToAction(
                "Index", "Customer",
                new { showAll  = true,
                      returnTo = "Wearer" });
        }

        // Load first wearer
        var first = await _api
            .GetFirstWearerAsync(model.CustId);

        if (first == null)
            model.Message =
                "No wearers found for this customer.";
        else
            model = Fill(model, first);

        return View("Index", model);
    }

    [HttpGet]
    public async Task<IActionResult> IndexWithCustomer(
        int custId, int custNbr, string? name,
        int route, string? gid)
    {
        HttpContext.Session.SetInt32(
            "SelectedCustId",  custId);
        HttpContext.Session.SetInt32(
            "SelectedCustNbr", custNbr);
        HttpContext.Session.SetString(
            "SelectedName",    name ?? "");
        HttpContext.Session.SetInt32(
            "SelectedRoute",   route);
        HttpContext.Session.SetString(
            "SelectedGID",     gid ?? "");

        var model = new WearerViewModel
        {
            CustId   = custId,
            CustNbr  = custNbr,
            CustName = name,
            Route    = route,
            GID      = gid
        };

        var first = await _api
            .GetFirstWearerAsync(custId);

        if (first == null)
            model.Message =
                "No wearers found for this customer.";
        else
            model = Fill(model, first);

        return View("Index", model);
    }

    [HttpGet]
    public async Task<IActionResult> SelectWearer()
    {
        var custId = HttpContext.Session
            .GetInt32("SelectedCustId") ?? 0;

        if (custId == 0)
        {
            return RedirectToAction(
                "Index", "Customer",
                new { showAll  = true,
                      returnTo = "Wearer" });
        }

        var wearers = await _api
            .GetAllWearersAsync(custId);

        ViewBag.CustId   = custId;
        ViewBag.CustName = HttpContext.Session
            .GetString("SelectedName");

        return View(wearers);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PickWearer(
        int custId, string wearNbr)
    {
        var model = FromSession();

        var found = await _api
            .GetWearerByNbrAsync(custId, wearNbr);

        if (found == null)
            model.Message =
                $"Wearer '{wearNbr}' not found.";
        else
            model = Fill(model, found);

        return View("Index", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Next(
        int custId, int currentWearerId)
    {
        var model = FromSession();

        var next = await _api
            .GetNextWearerAsync(custId, currentWearerId);

        if (next == null)
        {
            model.Message = "No more wearers.";
            var first = await _api
                .GetFirstWearerAsync(custId);
            if (first != null)
                model = Fill(model, first);
        }
        else
            model = Fill(model, next);

        return View("Index", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Prev(
        int custId, int currentWearerId)
    {
        var model = FromSession();

        var prev = await _api
            .GetPrevWearerAsync(custId, currentWearerId);

        if (prev == null)
        {
            model.Message = "Already at first wearer.";
            var first = await _api
                .GetFirstWearerAsync(custId);
            if (first != null)
                model = Fill(model, first);
        }
        else
            model = Fill(model, prev);

        return View("Index", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(
        int custId, int wearerId, string wearNbr,
        bool isMale, string? locker, string? lockRm)
    {
        var success = await _api.SaveWearerAsync(
            wearerId,
            locker  ?? string.Empty,
            lockRm  ?? string.Empty,
            isMale);

        TempData[success ? "Success" : "Error"] =
            success
            ? "Wearer saved successfully!"
            : "Failed to save wearer.";

        // Reload same wearer after save
        var model = FromSession();
        var first = await _api
            .GetFirstWearerAsync(custId);
        if (first != null)
            model = Fill(model, first);

        return View("Index", model);
    }

    [HttpGet]
    public IActionResult Exit()
    {
        return RedirectToAction(
            "Index", "CustomerAdmin");
    }
}