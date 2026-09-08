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
    int custId,
    int custNbr = 0,
    string? name = null,
    int route = 0,
    string? gid = null)
    {
        // Store selected customer
        HttpContext.Session.SetInt32(
            "SelectedCustId",
            custId);

        var settings =
            await _api.GetCTSSettingsAsync(custId);

        CTSSettingsViewModel model;

        if (settings != null)
        {
            // IMPORTANT:
            // Use database/API values as the main source.
            model = settings;
        }
        else
        {
            // Fallback only when API has no record
            model = new CTSSettingsViewModel
            {
                CustId = custId,
                CustNbr = custNbr,
                Name = name,
                Route = route,
                GID = gid
            };
        }

        // Keep session synchronized with actual loaded customer
        HttpContext.Session.SetInt32(
            "SelectedCustNbr",
            model.CustNbr);

        HttpContext.Session.SetString(
            "SelectedName",
            model.Name ?? "");

        HttpContext.Session.SetInt32(
            "SelectedRoute",
            model.Route);

        HttpContext.Session.SetString(
            "SelectedGID",
            model.GID ?? "");

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