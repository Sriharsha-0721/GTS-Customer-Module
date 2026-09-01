using GTSCustomerAPI.Web.Models;
using GTSCustomerAPI.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.Web.Controllers;

public class CTSSettingsController : Controller
{
    private readonly ApiService _api;

    public CTSSettingsController(ApiService api)
    {
        _api = api;
    }

    public IActionResult Index()
    {
        var custId = HttpContext.Session
            .GetInt32("SelectedCustId");

        if (!custId.HasValue || custId == 0)
        {
            // No customer — show customer list
            // with CTS Settings tab active
            return RedirectToAction(
                "Index", "Customer",
                new { showAll  = true,
                      returnTo = "CTSSettings" });
        }

        return RedirectToAction(
            nameof(IndexWithCustomer), new
            {
                custId  = custId,
                custNbr = HttpContext.Session
                    .GetInt32("SelectedCustNbr"),
                name    = HttpContext.Session
                    .GetString("SelectedName"),
                route   = HttpContext.Session
                    .GetInt32("SelectedRoute"),
                gid     = HttpContext.Session
                    .GetString("SelectedGID")
            });
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

        var settings =
            await _api.GetCTSSettingsAsync(custId);
        var model = settings
            ?? new CTSSettingsViewModel();

        model.CustId  = custId;
        model.CustNbr = custNbr;
        model.Name    = name;
        model.Route   = route;
        model.GID     = gid;

        return View("Index", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(
        CTSSettingsViewModel model)
    {
        var success =
            await _api.SaveCTSSettingsAsync(model);

        TempData[success ? "Success" : "Error"] =
            success
            ? $"CTS Settings saved!"
            : "Failed to save.";

        return RedirectToAction(
            nameof(IndexWithCustomer), new
            {
                custId  = model.CustId,
                custNbr = model.CustNbr,
                name    = model.Name,
                route   = model.Route,
                gid     = model.GID
            });
    }

    [HttpGet]
    public IActionResult Exit()
    {
        return RedirectToAction(
            "Index", "CustomerAdmin");
    }
}