using GTSCustomerAPI.Web.Models;
using GTSCustomerAPI.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.Web.Controllers;

public class CustomerAdminController : Controller
{
    private readonly ApiService _api;

    public CustomerAdminController(ApiService api)
    {
        _api = api;
    }

    private CustomerAdminViewModel FromSession() => new()
    {
        CustId = HttpContext.Session
            .GetInt32("SelectedCustId") ?? 0,
        CustNbr = HttpContext.Session
            .GetInt32("SelectedCustNbr") ?? 0,
        Name = HttpContext.Session
            .GetString("SelectedName"),
        Route = HttpContext.Session
            .GetInt32("SelectedRoute") ?? 0,
        GID = HttpContext.Session
            .GetString("SelectedGID")
    };

    public async Task<IActionResult> Index()
    {
        var model = FromSession();

        if (model.CustId == 0)
        {
            // No customer — show customer list
            // with Customer tab active
            return RedirectToAction(
                "Index", "Customer",
                new
                {
                    showAll = true,
                    returnTo = "CustomerAdmin"
                });
        }

        var flags = await _api
            .GetCustomerAdminAsync(model.CustId);
        if (flags != null)
        {
            model.OSSFlag = flags.OSSFlag;
            model.STFlag = flags.STFlag;
        }

        return View("Index", model);
    }

    [HttpGet]
    public async Task<IActionResult> IndexWithCustomer(
        int custId, int custNbr, string? name,
        int route, string? gid)
    {
        HttpContext.Session.SetInt32(
            "SelectedCustId", custId);
        HttpContext.Session.SetInt32(
            "SelectedCustNbr", custNbr);
        HttpContext.Session.SetString(
            "SelectedName", name ?? "");
        HttpContext.Session.SetInt32(
            "SelectedRoute", route);
        HttpContext.Session.SetString(
            "SelectedGID", gid ?? "");

        var model = new CustomerAdminViewModel
        {
            CustId = custId,
            CustNbr = custNbr,
            Name = name,
            Route = route,
            GID = gid
        };

        var flags = await _api
            .GetCustomerAdminAsync(custId);
        if (flags != null)
        {
            model.OSSFlag = flags.OSSFlag;
            model.STFlag = flags.STFlag;
        }

        return View("Index", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(
        CustomerAdminViewModel model)
    {
        var success = await _api
            .SaveCustomerAdminAsync(model);

        TempData[success ? "Success" : "Error"] =
            success
            ? $"Saved successfully for {model.Name}!"
            : "Failed to save. Check SP and table.";

        return RedirectToAction(
            nameof(IndexWithCustomer), new
            {
                custId = model.CustId,
                custNbr = model.CustNbr,
                name = model.Name,
                route = model.Route,
                gid = model.GID
            });
    }

    public IActionResult Exit()
    {
        return RedirectToAction("Index", "Home");
    }
}