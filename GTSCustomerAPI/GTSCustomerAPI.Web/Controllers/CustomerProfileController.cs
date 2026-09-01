using GTSCustomerAPI.Web.Models;
using GTSCustomerAPI.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.Web.Controllers;

public class CustomerProfileController : Controller
{
    private readonly ApiService _api;

    public CustomerProfileController(ApiService api)
    {
        _api = api;
    }

    // =====================================================
    // SESSION DATA
    // =====================================================

    private CustomerProfileViewModel FromSession()
    {
        return new CustomerProfileViewModel
        {
            CustId = HttpContext.Session
                .GetInt32("SelectedCustId") ?? 0,

            CustNbr = (
                HttpContext.Session
                    .GetInt32("SelectedCustNbr") ?? 0
            ).ToString(),

            CustName = HttpContext.Session
                .GetString("SelectedName"),

            Route = HttpContext.Session
                .GetInt32("SelectedRoute") ?? 0,

            GID = HttpContext.Session
                .GetString("SelectedGID"),

            MarketCenter = (
                HttpContext.Session
                    .GetInt32("SelectedMC") ?? 0
            ).ToString()
        };
    }

    // =====================================================
    // INDEX
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = FromSession();

        if (model.CustId == 0)
        {
            return RedirectToAction(
                "Index",
                "Customer",
                new
                {
                    returnTo = "CustomerProfile"
                });
        }

        await LoadAllData(model);

        model.ActiveSection =
            TempData["ActiveSection"]
                ?.ToString()
            ?? "Billing";

        return View("Index", model);
    }

    // =====================================================
    // INDEX WITH SELECTED CUSTOMER
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> IndexWithCustomer(
        int custId,
        int custNbr,
        string? name,
        int route,
        string? gid,
        int? marketCenter = null)
    {
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

        if (marketCenter.HasValue)
        {
            HttpContext.Session.SetInt32(
                "SelectedMC",
                marketCenter.Value);
        }

        var model = FromSession();

        await LoadAllData(model);

        model.ActiveSection =
            "Billing";

        return View("Index", model);
    }

    // =====================================================
    // LOAD CUSTOMER PROFILE + FORMULA + DOSAGE
    // =====================================================

    private async Task LoadAllData(
        CustomerProfileViewModel model)
    {
        // -------------------------------------------------
        // Customer Profile
        // -------------------------------------------------

        var data = await _api
            .GetCustomerProfileAsync(
                model.CustId);

        if (data != null)
        {
            Apply(model, data);
        }

        // -------------------------------------------------
        // MarketCenter conversion
        // -------------------------------------------------

        int marketCenter = 0;

        int.TryParse(
            model.MarketCenter,
            out marketCenter);

        // -------------------------------------------------
        // Wash Formula
        // -------------------------------------------------

        if (marketCenter > 0)
        {
            var formula = await _api
                .GetFormulaAsync(
                    marketCenter,
                    model.CustId);

            model.FormulaData =
                formula ?? string.Empty;
        }

        // -------------------------------------------------
        // Dosage
        // -------------------------------------------------

        var dosage = await _api
            .GetDosageAsync(
                model.CustId);

        if (dosage != null)
        {
            if (dosage.Dosage.HasValue)
            {
                if (dosage.Dosage.Value)
                {
                    // High Dose
                    model.HighDose = true;
                    model.LowDose = false;
                }
                else
                {
                    // Low Dose
                    model.HighDose = false;
                    model.LowDose = true;
                }
            }
            else
            {
                model.HighDose = false;
                model.LowDose = false;
            }
        }
    }

    // =====================================================
    // OVERALL CUSTOMER PROFILE SAVE
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(
        CustomerProfileViewModel model)
    {
        // -------------------------------------------------
        // 1. Save Customer Profile
        // BillingCom, PackoutCom, WashCom etc.
        // -------------------------------------------------

        var success = await _api
            .SaveCustomerProfileAsync(model);

        // -------------------------------------------------
        // 2. Convert MarketCenter
        // -------------------------------------------------

        int marketCenter = 0;

        int.TryParse(
            model.MarketCenter,
            out marketCenter);

        // -------------------------------------------------
        // 3. Save Wash Formula
        // -------------------------------------------------

        if (marketCenter > 0)
        {
            await _api.SaveFormulaAsync(
                marketCenter,
                model.CustId,
                model.FormulaData
                    ?? string.Empty);
        }

        // -------------------------------------------------
        // 4. Save Dosage
        // -------------------------------------------------

        bool? dosageValue = null;

        if (model.HighDose == true)
        {
            dosageValue = true;
        }
        else if (model.LowDose == true)
        {
            dosageValue = false;
        }

        if (dosageValue.HasValue)
        {
            await _api.UpdateDosageAsync(
                model.CustId,
                dosageValue.Value);
        }

        // -------------------------------------------------
        // Result
        // -------------------------------------------------

        TempData[
            success
                ? "Success"
                : "Error"
        ] =
            success
                ? "Customer Profile saved!"
                : "Failed to save Customer Profile.";

        // Restore the same profile section after redirect.
        // Example: Packout Save -> return to Packout.
        TempData["ActiveSection"] =
            string.IsNullOrWhiteSpace(
                model.ActiveSection)
                ? "Billing"
                : model.ActiveSection;

        return RedirectToAction(
            nameof(Index));
    }

    // =====================================================
    // BILLING - WEB AJAX ACTIONS
    // =====================================================

    // GET:
    // /CustomerProfile/GetBillingData
    [HttpGet]
    public async Task<IActionResult> GetBillingData()
    {
        var result =
            await _api.GetBillingTypesAsync();

        return Json(result);
    }

    // GET:
    // /CustomerProfile/GetBillingCharges?custId=26239
    [HttpGet]
    public async Task<IActionResult> GetBillingCharges(
        int custId)
    {
        var result =
            await _api.GetBillingChargesAsync(
                custId);

        return Json(result);
    }

    // POST:
    // /CustomerProfile/SaveBillingCharge
    [HttpPost]
    public async Task<IActionResult> SaveBillingCharge(
        [FromBody]
        BillingChargeSaveViewModel model)
    {
        if (model == null)
        {
            return BadRequest(
                "Billing data is required.");
        }

        if (model.CustId <= 0)
        {
            return BadRequest(
                "Invalid customer.");
        }

        var result =
            await _api.SaveBillingChargeAsync(
                model);

        if (result <= 0)
        {
            return BadRequest(
                "Failed to save billing charge.");
        }

        return Json(result);
    }

    // DELETE:
    // /CustomerProfile/DeleteBillingCharge?id=10
    [HttpDelete]
    public async Task<IActionResult> DeleteBillingCharge(
        int id)
    {
        if (id <= 0)
        {
            return BadRequest(
                "Invalid Billing Charge Id.");
        }

        var result =
            await _api.DeleteBillingChargeAsync(
                id);

        if (result <= 0)
        {
            return BadRequest(
                "Failed to delete billing charge.");
        }

        return Json(result);
    }

    // =====================================================
    // PACKOUT - WEB AJAX ACTIONS
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetPackoutDetails(
        int custId)
    {
        if (custId <= 0)
            return BadRequest(
                "Invalid customer.");

        var result =
            await _api
                .GetPackoutDetailsAsync(
                    custId);

        return Json(result);
    }


    [HttpGet]
    public async Task<IActionResult> GetPackoutItems(
        int custId)
    {
        if (custId <= 0)
            return BadRequest(
                "Invalid customer.");

        var result =
            await _api
                .GetPackoutItemsAsync(
                    custId);

        return Json(result);
    }


    [HttpPost]
    public async Task<IActionResult> SavePackout(
        [FromBody] PackoutViewModel model)
    {
        if (model == null)
            return BadRequest(
                "Packout data required.");

        if (model.CustId <= 0)
            return BadRequest(
                "Invalid customer.");

        var result =
            await _api
                .SavePackoutAsync(
                    model);

        if (result <= 0)
            return BadRequest(
                "Failed to save Packout.");

        return Json(result);
    }


    [HttpDelete]
    public async Task<IActionResult> DeletePackout(
        int id)
    {
        if (id <= 0)
            return BadRequest(
                "Invalid Packout Id.");

        var result =
            await _api
                .DeletePackoutAsync(
                    id);

        if (result <= 0)
            return BadRequest(
                "Failed to delete Packout.");

        return Json(result);
    }

    // =====================================================
    // WASH - GARMENT TYPES
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetGarmentTypes()
    {
        var result =
            await _api.GetGarmentTypesAsync();

        return Json(result);
    }
    // =====================================================
    // APPLY API DATA TO VIEW MODEL
    // =====================================================

    private static void Apply(
        CustomerProfileViewModel model,
        CustomerProfileViewModel data)
    {
        model.SterileCode =
            data.SterileCode;

        model.CtmndFlg =
            data.CtmndFlg;

        model.DelTicket =
            data.DelTicket;

        model.PropertyMark =
            data.PropertyMark;

        model.ShipVia =
            data.ShipVia;

        model.Package =
            data.Package;

        model.OSSFlag =
            data.OSSFlag;

        model.WashCom =
            data.WashCom;

        model.DryerCom =
            data.DryerCom;

        model.MainCleanRoomCom =
            data.MainCleanRoomCom;

        model.PackoutCom =
            data.PackoutCom;

        model.ShippingCom =
            data.ShippingCom;

        model.DriverCom =
            data.DriverCom;

        model.MendCom =
            data.MendCom;

        model.QACom =
            data.QACom;

        model.CustSrvCom =
            data.CustSrvCom;

        model.BillingCom =
            data.BillingCom;

        model.QAInspCom =
            data.QAInspCom;

        model.OfficeCom =
            data.OfficeCom;

        model.ProdCom =
            data.ProdCom;

        model.GenOfficeCom =
            data.GenOfficeCom;

        model.MerControlCom =
            data.MerControlCom;

        model.ReceivingCom =
            data.ReceivingCom;

        model.SoilCom =
            data.SoilCom;

        if (!string.IsNullOrWhiteSpace(
            data.MarketCenter))
        {
            model.MarketCenter =
                data.MarketCenter;
        }
    }
}