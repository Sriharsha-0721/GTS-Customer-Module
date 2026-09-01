
﻿using GTSCustomerAPI.Web.Models;
using System.Text;
using System.Text.Json;

namespace GTSCustomerAPI.Web.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true
    };
    public ApiService(HttpClient http) { _http = http; }

    // ── CUSTOMERS ─────────────────────────────────────────

    public async Task<List<CustomerViewModel>> GetAllAsync()
    {
        var res = await _http.GetAsync("api/customers");
        if (!res.IsSuccessStatusCode)
            return new List<CustomerViewModel>();
        var body = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<CustomerViewModel>>(body, _json)
        ?? new List<CustomerViewModel>();
    }

    public async Task<CustomerViewModel?> GetByIdAsync(
        int id)
    {
        var res = await _http
            .GetAsync($"api/customers/{id}");
        if (!res.IsSuccessStatusCode) return null;
        var body = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CustomerViewModel>(body, _json);
    }

    public async Task<CustomerViewModel?> CreateAsync(
        CreateCustomerViewModel model)
    {
        var body = JsonSerializer.Serialize(model);
        var ct = new StringContent(
            body, Encoding.UTF8, "application/json");
        var res = await _http
            .PostAsync("api/customers", ct);
        if (!res.IsSuccessStatusCode) return null;
        var rb = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CustomerViewModel>(body, _json);
    }

    public async Task<bool> UpdateAsync(
        int id, UpdateCustomerViewModel model)
    {
        var body = JsonSerializer.Serialize(model);
        var ct = new StringContent(
            body, Encoding.UTF8, "application/json");
        var res = await _http
            .PutAsync($"api/customers/{id}", ct);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var res = await _http
            .DeleteAsync($"api/customers/{id}");
        return res.IsSuccessStatusCode;
    }

    // ── CTS SETTINGS ──────────────────────────────────────

    public async Task<CTSSettingsViewModel?>
        GetCTSSettingsAsync(int custId)
    {
        var res = await _http
            .GetAsync($"api/ctssettings/{custId}");
        if (!res.IsSuccessStatusCode) return null;
        var body = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CTSSettingsViewModel>(body, _json);
    }

    public async Task<bool> SaveCTSSettingsAsync(
        CTSSettingsViewModel model)
    {
        var payload = new
        {
            model.CustId,
            model.PrintIssueStatusFlag,
            model.PrintBornonDateFlag,
            model.NOGFlag,
            model.LabelHeader
        };
        var body = JsonSerializer.Serialize(payload);
        var ct = new StringContent(
            body, Encoding.UTF8, "application/json");
        var res = await _http
            .PostAsync("api/ctssettings", ct);
        return res.IsSuccessStatusCode;
    }

    // ── CUSTOMER ADMIN ────────────────────────────────────

    public async Task<CustomerAdminViewModel?>
        GetCustomerAdminAsync(int custId)
    {
        var res = await _http
            .GetAsync($"api/customeradmin/{custId}");
        if (!res.IsSuccessStatusCode) return null;
        var body = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CustomerAdminViewModel>(body, _json);
    }

    public async Task<bool> SaveCustomerAdminAsync(
        CustomerAdminViewModel model)
    {
        var payload = new
        {
            model.CustId,
            model.OSSFlag,
            model.STFlag
        };
        var body = JsonSerializer.Serialize(payload);
        var ct = new StringContent(
            body, Encoding.UTF8, "application/json");
        var res = await _http
            .PostAsync("api/customeradmin", ct);
        return res.IsSuccessStatusCode;
    }

    // ── WEARER ────────────────────────────────────────────

    public async Task<WearerViewModel?> GetFirstWearerAsync(
        int custId)
    {
        var res = await _http
            .GetAsync($"api/wearer/{custId}/first");
        if (!res.IsSuccessStatusCode) return null;
        var body = await res.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<List<WearerViewModel>>(body, _json);
        return list?.FirstOrDefault();
    }

    public async Task<WearerViewModel?> GetNextWearerAsync(
        int custId, int lastWrId)
    {
        var res = await _http.GetAsync(
            $"api/wearer/{custId}/next/{lastWrId}");
        if (!res.IsSuccessStatusCode) return null;
        var body = await res.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<List<WearerViewModel>>(body, _json);
        return list?.FirstOrDefault();
    }

    public async Task<WearerViewModel?> GetPrevWearerAsync(
        int custId, int firstWrId)
    {
        var res = await _http.GetAsync(
            $"api/wearer/{custId}/prev/{firstWrId}");
        if (!res.IsSuccessStatusCode) return null;
        var body = await res.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<List<WearerViewModel>>(body, _json);
        return list?.FirstOrDefault();
    }

    public async Task<WearerViewModel?> GetWearerByNbrAsync(
        int custId, string wearNbr)
    {
        var res = await _http.GetAsync(
            $"api/wearer/{custId}/search/{wearNbr}");
        if (!res.IsSuccessStatusCode) return null;
        var body = await res.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<List<WearerViewModel>>(body, _json);
        return list?.FirstOrDefault();
    }

    public async Task<List<WearerViewModel>>
        GetAllWearersAsync(int custId)
    {
        var res = await _http
            .GetAsync($"api/wearer/{custId}/all");
        if (!res.IsSuccessStatusCode)
            return new List<WearerViewModel>();
        var body = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<WearerViewModel>>(body, _json)
       ?? new List<WearerViewModel>();
    }

    public async Task<bool> SaveWearerAsync(
        int wearerId, string locker,
        string lockRm, bool isMale)
    {
        var payload = new
        {
            WearerId = wearerId,
            Locker = locker,
            LockRm = lockRm,
            IsMale = isMale
        };
        var body = JsonSerializer.Serialize(payload);
        var ct = new StringContent(
            body, Encoding.UTF8, "application/json");
        var res = await _http
            .PostAsync("api/wearer", ct);
        return res.IsSuccessStatusCode;
    }

    public async Task<CustomerProfileViewModel?>
    GetCustomerProfileAsync(int custId)
    {
        var res = await _http.GetAsync(
            $"api/customerprofile/{custId}");
        if (!res.IsSuccessStatusCode) return null;
        var body = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CustomerProfileViewModel>(body, _json);
    }

    public async Task<bool> SaveCustomerProfileAsync(
        CustomerProfileViewModel m)
    {
        var payload = new
        {
            CustID = m.CustId,
            SterileCode = m.SterileCode,
            CtmndFlg = m.CtmndFlg != 0,
            DelTicket = m.DelTicket != 0,
            PropertyMark = m.PropertyMark,
            ShipVia = m.ShipVia,
            Package = m.Package,
            ProdCom = m.ProdCom,
            OfficeCom = m.OfficeCom,
            GenOfficeCom = m.GenOfficeCom,
            MerControlCom = m.MerControlCom,
            ReceivingCom = m.ReceivingCom,
            SoilCom = m.SoilCom,
            WashCom = m.WashCom,
            DryerCom = m.DryerCom,
            MainCleanRoomCom = m.MainCleanRoomCom,
            PackoutCom = m.PackoutCom,
            ShippingCom = m.ShippingCom,
            DriverCom = m.DriverCom,
            MendCom = m.MendCom,
            QACom = m.QACom,
            CustSrvCom = m.CustSrvCom,
            BillingCom = m.BillingCom,
            UserID = 1,
            OSSFlag = m.OSSFlag,
            QAInspCom = m.QAInspCom
        };
        var body = JsonSerializer.Serialize(payload);
        var ct = new StringContent(
            body, System.Text.Encoding.UTF8,
            "application/json");
        var res = await _http.PostAsync(
            "api/customerprofile", ct);
        return res.IsSuccessStatusCode;
    }

    // ── BILLING ────────────────────────────────────────

    // ── BILLING ────────────────────────────────────────

    // GET /api/billing/BillingData
    public async Task<List<BillingDataViewModel>>
      GetBillingTypesAsync()
    {
        var res = await _http.GetAsync(
            "api/billing/BillingData");

        var responseBody =
            await res.Content.ReadAsStringAsync();

        Console.WriteLine(
            $"Billing API Status: {res.StatusCode}");

        Console.WriteLine(
            $"Billing API Response: {responseBody}");

        if (!res.IsSuccessStatusCode)
        {
            return new List<BillingDataViewModel>();
        }

        return JsonSerializer.Deserialize<
            List<BillingDataViewModel>>(
                responseBody,
                _json)
            ?? new List<BillingDataViewModel>();
    }
    // GET /api/billing/{custId}
    public async Task<List<BillingChargeViewModel>>
        GetBillingChargesAsync(int custId)
    {
        var res = await _http.GetAsync(
            $"api/billing/{custId}");
        if (!res.IsSuccessStatusCode)
            return new List<BillingChargeViewModel>();
        var body = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<BillingChargeViewModel>>(body, _json)
        ?? new List<BillingChargeViewModel>();
    }

    // POST /api/billing
    public async Task<int> SaveBillingChargeAsync(
        BillingChargeSaveViewModel dto)
    {
        var body = JsonSerializer.Serialize(dto);
        var content = new StringContent(
            body,
            System.Text.Encoding.UTF8,
            "application/json");
        var res = await _http.PostAsync(
            "api/billing", content);
        if (!res.IsSuccessStatusCode) return 0;
        var result =
            await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<int>(result);
    }

    // DELETE /api/billing/{id}
    public async Task<int> DeleteBillingChargeAsync(
        int billingChargesId)
    {
        var res = await _http.DeleteAsync(
            $"api/billing/{billingChargesId}");
        if (!res.IsSuccessStatusCode) return 0;
        var result =
            await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<int>(result);
    }

    // ── PACKOUT ────────────────────────────────────────

    public async Task<List<PackoutViewModel>>
    GetPackoutDetailsAsync(int custId)
    {
        var res =
            await _http.GetAsync(
                $"api/packout/{custId}");

        if (!res.IsSuccessStatusCode)
            return new();

        var body =
            await res.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<
            List<PackoutViewModel>>(
                body,
                _json)
            ?? new();
    }


    public async Task<List<string>>
        GetPackoutItemsAsync(int custId)
    {
        var res =
            await _http.GetAsync(
                $"api/packout/Items/{custId}");

        if (!res.IsSuccessStatusCode)
            return new();

        var body =
            await res.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<
            List<string>>(
                body,
                _json)
            ?? new();
    }


    public async Task<int> SavePackoutAsync(
        PackoutViewModel dto)
    {
        var body =
            JsonSerializer.Serialize(dto);

        var content =
            new StringContent(
                body,
                System.Text.Encoding.UTF8,
                "application/json");

        var res =
            await _http.PostAsync(
                "api/packout",
                content);

        if (!res.IsSuccessStatusCode)
            return 0;

        var result =
            await res.Content
                .ReadAsStringAsync();

        return JsonSerializer
            .Deserialize<int>(
                result);
    }


    public async Task<int> DeletePackoutAsync(
        int id)
    {
        var res =
            await _http.DeleteAsync(
                $"api/packout/{id}");

        if (!res.IsSuccessStatusCode)
            return 0;

        var result =
            await res.Content
                .ReadAsStringAsync();

        return JsonSerializer
            .Deserialize<int>(
                result);
    }
    // ── DOSAGE ────────────────────────────────────────

    public async Task<CustomerDosageViewModel?>
        GetDosageAsync(int custId)
    {
        var res = await _http.GetAsync(
            $"api/dosage/{custId}");
        if (!res.IsSuccessStatusCode) return null;
        var body = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CustomerDosageViewModel>(body, _json);
    }

    public async Task UpdateDosageAsync(
        int custId, bool? dosage)
    {
        var payload = new
        {
            CustId = custId,
            Dosage = dosage
        };
        var body = JsonSerializer.Serialize(payload);
        var content = new StringContent(
            body,
            System.Text.Encoding.UTF8,
            "application/json");
        await _http.PostAsync(
            "api/dosage/update", content);
    }

    // ── WASH / FORMULA ────────────────────────────────

    public async Task<string> GetFormulaAsync(
        int mc, int custId)
    {
        // If MC is 0, skip the call
        if (custId == 0) return "";

        var res = await _http.GetAsync(
            $"api/wash/formula/{mc}/{custId}");
        if (!res.IsSuccessStatusCode) return "";

        var body = await res.Content.ReadAsStringAsync();
        try
        {
            var obj = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            body, _json);
            return obj != null
                   && obj.ContainsKey("formula")
                ? obj["formula"].GetString() ?? ""
                : "";
        }
        catch { return ""; }
    }

    public async Task<int> SaveFormulaAsync(
        int mc, int custId, string formulaString)
    {
        if (custId == 0) return 0;

        var payload = new
        {
            MarketCenter = mc,
            CustId = custId,
            FormulaString = formulaString
        };
        var body = JsonSerializer.Serialize(payload);
        var content = new StringContent(
            body,
            System.Text.Encoding.UTF8,
            "application/json");
        var res = await _http.PostAsync(
            "api/wash/save-formula", content);
        if (!res.IsSuccessStatusCode) return 0;
        var result = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<int>(result);
    }

    public async Task<List<GarmentTypeViewModel>>
        GetGarmentTypesAsync()
    {
        var res = await _http.GetAsync(
            "api/wash/garmenttypes");
        if (!res.IsSuccessStatusCode)
            return new List<GarmentTypeViewModel>();
        var body = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<GarmentTypeViewModel>>(body, _json)
        ?? new List<GarmentTypeViewModel>();
    }
}



