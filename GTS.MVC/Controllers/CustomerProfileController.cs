using GTS.Application.DTOs;
using GTS.Application.Interfaces.CustomerServices;
using GTS.Domain.Entities;
using GTS.MVC.Models;
using GTS.MVC.Models.Billing;
using GTS.MVC.Models.CTSSettings;
using GTS.MVC.Models.CustomerLineComments;
using GTS.MVC.Models.CustomerProfile;
using GTS.MVC.Models.MaximumWash;
using GTS.MVC.Models.Packout;
using GTS.MVC.Models.SpecialLines;
using GTS.MVC.Models.Wash;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;

namespace GTS.MVC.Controllers
{
    public class CustomerProfileController : Controller
    {
        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

        private readonly HttpClient _http;
        private readonly ICustomerService _customerService;

        public CustomerProfileController(IHttpClientFactory httpClientFactory, ICustomerService customerService)
        {
            _http = httpClientFactory.CreateClient("GtsApiClient");
            _customerService = customerService;
        }
        public IActionResult Index()
        {
            ViewBag.ActiveTab = "";
            return View();
        }

        // =========================================================
        // LEGACY SP SEARCH INTEGRATION FOR THE MODAL
        // =========================================================
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Search(
            int? searchId,
            string? searchName,
            int? route,
            int? marketCenter,
            bool showAll = false,
            [FromBody] CustomerFilterRequestDto? postFilter = null,
            CancellationToken ct = default)
        {
            int mcToUse = (marketCenter.HasValue && marketCenter.Value > 0)
                ? marketCenter.Value
                : ((postFilter != null && postFilter.MarketCenter > 0) ? postFilter.MarketCenter : 561);

            var filter = postFilter ?? new CustomerFilterRequestDto
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

            var customers = (await _customerService.GetCustomersBasedOnFilter(filter, ct)).ToList();

            if (customers.Count == 0)
            {
                ViewBag.Error = $"No customers found for Market Center {mcToUse}.";
            }

            return PartialView("~/Views/Customer/_Search.cshtml", customers);
        }

        public IActionResult Blank()
        {
            return PartialView("_Blank");
        }

        // =========================================================
        // CUSTOMER PROFILE SUBMODULE
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> Profile(int custId, CancellationToken ct = default)
        {
            var model = new CustomerViewModel { CustId = custId };

            if (custId > 0)
            {
                try
                {
                    var response = await _http.GetAsync($"api/Customer/customer-profile/{custId}", ct);
                    if (response.IsSuccessStatusCode)
                    {
                        var dto = await response.Content.ReadFromJsonAsync<CustomerProfileDto>(JsonOptions, cancellationToken: ct);
                        if (dto != null)
                        {
                            model.CustId = dto.CustId;
                            model.CustNo = dto.CustNbr;
                            model.Name = dto.Name;
                            model.Route = dto.Route;
                            model.Gid = dto.GID;
                            model.MC = dto.MarketCenter;
                            model.BillingCom = dto.BillingCom ?? "";
                            model.PackoutCom = dto.PackoutCom ?? "";
                            model.WashCom = dto.WashCom ?? "";
                            model.SoilCom = dto.SoilCom ?? "";
                            model.DryerCom = dto.DryerCom ?? "";
                            model.ReceivingCom = dto.ReceivingCom ?? "";
                            model.ShippingCom = dto.ShippingCom ?? "";
                            model.DriverCom = dto.DriverCom ?? "";
                            model.MendCom = dto.MendCom ?? "";
                            model.QACom = dto.QACom ?? "";
                            model.CustSrvCom = dto.CustSrvCom ?? "";
                            model.OfficeCom = dto.OfficeCom ?? "";
                            model.GenOfficeCom = dto.GenOfficeCom ?? "";
                            model.MerControlCom = dto.MerControlCom ?? "";
                            model.MainCleanRoomCom = dto.MainCleanRoomCom ?? "";
                            model.QAInspCom = dto.QAInspCom ?? "";
                            model.ProdCom = dto.ProdCom ?? "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[GTS Profile Controller Exception]: {ex.Message}");
                }
            }

            return PartialView("_Profile", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomerProfile([FromBody] UpdateCustomerProfileDto model, CancellationToken ct = default)
        {
            var response = await _http.PutAsJsonAsync("api/Customer/update-profile", model, ct);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                return BadRequest(error);
            }

            return Ok();
        }

        // =========================================================
        // CUSTOMER FLAGS SUBMODULE
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> Customer(int custId, CancellationToken ct = default)
        {
            if (custId <= 0) return NotFound();

            var model = new CustomerViewModel { CustId = custId };

            try
            {
                var profResp = await _http.GetAsync($"api/Customer/customer-profile/{custId}", ct);
                if (profResp.IsSuccessStatusCode)
                {
                    var prof = await profResp.Content.ReadFromJsonAsync<CustomerProfileDto>(JsonOptions, ct);
                    if (prof != null)
                    {
                        model.CustNo = prof.CustNbr;
                        model.Name = prof.Name;
                        model.Route = prof.Route;
                        model.Gid = prof.GID;
                        model.MC = prof.MarketCenter;
                    }
                }

                var flagResp = await _http.GetAsync($"api/Customer/customer-flagdetails/{custId}", ct);
                if (flagResp.IsSuccessStatusCode)
                {
                    var flags = await flagResp.Content.ReadFromJsonAsync<CustomerFlagsDto>(JsonOptions, ct);
                    if (flags != null)
                    {
                        model.OSSFlag = flags.OSSFlag;
                        model.STFlag = flags.STFFlag;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Customer Action Error]: {ex.Message}");
            }

            return PartialView("Customer", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomerFlags([FromBody] CustomerViewModel model, CancellationToken ct = default)
        {
            var response = await _http.PostAsync($"api/Customer/update-flagdetails/{model.CustId}/{model.OSSFlag}/{model.STFlag}", null, ct);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                return BadRequest(error);
            }

            return Ok();
        }

        // =========================================================
        // CTS SETTINGS SUBMODULE (API Driven)
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> CTSSettings(int custId, CancellationToken ct = default)
        {
            if (custId <= 0) return NotFound();

            // 1. Fetch customer details
            var custResp = await _http.GetAsync($"api/Customer/customer-profile/{custId}", ct);
            if (!custResp.IsSuccessStatusCode) return NotFound("Customer profile not found.");

            var customer = await custResp.Content.ReadFromJsonAsync<CustomerProfileDto>(JsonOptions, ct);
            if (customer == null) return NotFound("Customer record empty.");

            // 2. Fetch CTS Settings using CustNbr
            var ctsResp = await _http.GetAsync($"api/CTSSetting/{customer.CustNbr}", ct);
            CTSSettingsViewModel model = new();

            if (ctsResp.IsSuccessStatusCode)
            {
                var list = await ctsResp.Content.ReadFromJsonAsync<List<CTSSettingsViewModel>>(JsonOptions, ct);
                model = list?.FirstOrDefault() ?? new CTSSettingsViewModel();
            }

            // Populate header details onto the view model
            model.MC = customer.MarketCenter;
            model.CustNo = customer.CustNbr;
            model.Customer = customer.Name;
            model.Route = customer.Route;
            model.GID = customer.GID;

            ViewBag.CustNo = customer.CustNbr;
            ViewBag.Customer = customer.Name;
            ViewBag.Route = customer.Route;
            ViewBag.GID = customer.GID;
            ViewBag.MC = customer.MarketCenter;

            return PartialView("_CTSSettings", model);
        }

        // =========================================================
        // SPECIAL LINES SUBMODULE
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> SpecialLines(int custId)
        {
            List<SpecialLineViewModel> allLines = [];

            if (custId > 0)
            {
                var pageResult = await _http.GetFromJsonAsync<SpecialLinePageViewModel>(
                    $"api/SpecialLines/{custId}?includeAll=true&pageSize=10000") ?? new();

                allLines = pageResult.Items ?? [];

                foreach (var item in allLines)
                {
                    if (item.CustId == custId || item.CustId > 0)
                    {
                        item.IsSelected = true;
                    }
                }
            }

            return PartialView("_SpecialLines", allLines);
        }

        [HttpPost]
        public async Task<IActionResult> AddSpecialLine(int custId, int line)
        {
            var response = await _http.PostAsync($"api/SpecialLines/{custId}/{line}", null);
            var result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSpecialLine(int line)
        {
            var response = await _http.DeleteAsync($"api/SpecialLines/{line}");
            var result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSpecialLines([FromBody] SaveSpecialLinesViewModel model)
        {
            var response = await _http.PostAsJsonAsync("api/SpecialLines", model);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, error);
            }
            return Ok();
        }

        // =========================================================
        // BILLING SUBMODULE
        // =========================================================
        public async Task<IActionResult> Billing(int custId)
        {
            BillingViewModel model = new();

            if (custId > 0)
            {
                model.Charges = await _http.GetFromJsonAsync<List<BillingChargeViewModel>>($"api/Billing/{custId}") ?? [];
                model.ChargeTypes = await _http.GetFromJsonAsync<List<BillingDataViewModel>>("api/Billing/BillingData") ?? [];
            }

            return PartialView("_Billing", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBilling([FromBody] BillingChargeViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _http.PostAsJsonAsync("api/Billing", model);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, result);

            return Content(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBilling(int billingChargesId)
        {
            var response = await _http.DeleteAsync($"api/Billing/{billingChargesId}");
            var result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }

        // =========================================================
        // PACKOUT SUBMODULE
        // =========================================================
        public async Task<IActionResult> Packout(int custId)
        {
            PackoutViewModel model = new();

            if (custId > 0)
            {
                model.PackoutDetails = await _http.GetFromJsonAsync<List<PackoutDetailsViewModel>>($"api/Packout/{custId}") ?? [];
                var itemCodes = await _http.GetFromJsonAsync<List<string>>($"api/Packout/Items/{custId}") ?? [];
                model.Items = itemCodes.Select(x => new PackoutItemViewModel { Item = x }).ToList();
            }

            return PartialView("_Packout", model);
        }

        [HttpPost]
        public async Task<IActionResult> SavePackout([FromBody] PackoutSaveViewModel model)
        {
            var response = await _http.PostAsJsonAsync("api/Packout", model);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, result);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePackout(int pkoutRestrictId)
        {
            try
            {
                if (pkoutRestrictId <= 0) return BadRequest("Invalid Packout ID.");

                var response = await _http.DeleteAsync($"api/Packout/{pkoutRestrictId}");
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================================================
        // WASH SUBMODULE
        // =========================================================
        public async Task<IActionResult> Wash(int custId)
        {
            WashViewModel model = new();

            if (custId > 0)
            {
                var wash = await _http.GetFromJsonAsync<WashDTO>(
                    $"api/Wash/{custId}") ?? new WashDTO();
                model.CustId = wash.CustId;
                model.WashCom = wash.WashCom ?? "";

                if (!string.IsNullOrWhiteSpace(wash.Formula))
                {
                    var values = wash.Formula.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i + 1 < values.Length; i += 2)
                    {
                        model.Formulas.Add(new WashFormulaViewModel
                        {
                            GarmentType = values[i].Trim(),
                            Formula = values[i + 1].Trim()
                        });
                    }
                }

                model.GarmentTypes = await _http.GetFromJsonAsync<List<string>>("api/Wash/GarmentTypes") ?? [];
            }

            return PartialView("_Wash", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveWash([FromBody] WashDTO model)
        {
            var response = await _http.PostAsJsonAsync("api/Wash", model);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, result);

            return Ok(result);
        }

        // =========================================================
        // MAXIMUM WASH
        // =========================================================
        public async Task<IActionResult> MaximumWash(int custId)
        {
            List<MaximumWashViewModel> model = [];

            if (custId > 0)
            {
                model = await _http.GetFromJsonAsync<List<MaximumWashViewModel>>($"api/MaxWash/{custId}") ?? [];
            }

            return PartialView("_MaximumWashGrid", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddMaximumWash([FromBody] MaximumWashViewModel model)
        {
            var dto = new
            {
                model.CustId,
                model.ItemCode,
                model.MaxWash,
                model.MaxWeeks,
                model.MaxCycles
            };

            var response = await _http.PostAsJsonAsync("api/MaxWash", dto);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, result);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMaximumWash([FromBody] MaximumWashViewModel model)
        {
            var dto = new
            {
                model.CustId,
                model.OldItemCode,
                model.NewItemCode,
                model.MaxWash,
                model.MaxWeeks,
                model.MaxCycles
            };

            var response = await _http.PostAsJsonAsync("api/MaxWash", dto);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, result);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMaximumWash(int custId, string itemCode)
        {
            var response = await _http.DeleteAsync($"api/MaxWash/{custId}/{itemCode}");
            var result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }

        // =========================================================
        // CUSTOMER LINE COMMENTS
        // =========================================================
        public async Task<IActionResult> CustomerLineComments(int custId)
        {
            List<CustomerLineCommentsViewModel> model = [];

            if (custId > 0)
            {
                model = await _http.GetFromJsonAsync<List<CustomerLineCommentsViewModel>>($"api/CustomerLineComments/{custId}") ?? [];
            }

            return PartialView("_CustomerLineComments", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomerLineComments([FromBody] CustomerLineCommentsViewModel model)
        {
            var dto = new
            {
                model.CustId,
                model.WearItemId,
                model.Descr
            };

            var response = await _http.PostAsJsonAsync("api/CustomerLineComments", dto);
            var result = await response.Content.ReadAsStringAsync();

            return Ok(result);
        }

        // =========================================================
        // WEARER SUBMODULE
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> Wearer(int custId)
        {
            if (custId <= 0) return BadRequest("Customer is required.");

            var response = await _http.GetAsync($"api/Wearer/Next?custId={custId}&wearerId=");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return PartialView("Wearer", new WearerViewModel { CustId = custId });
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, error);
            }

            var model = await response.Content.ReadFromJsonAsync<WearerViewModel>() ?? new WearerViewModel();
            model.CustId = custId;
            return PartialView("Wearer", model);
        }

        [HttpGet]
        public async Task<IActionResult> WearerPrevious(int custId, int wearerId)
        {
            var response = await _http.GetAsync($"api/Wearer/Previous?custId={custId}&wearerId={wearerId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return NoContent();

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, error);
            }

            var model = await response.Content.ReadFromJsonAsync<WearerViewModel>();
            if (model == null) return NoContent();

            model.CustId = custId;
            return PartialView("Wearer", model);
        }

        [HttpGet]
        public async Task<IActionResult> WearerNext(int custId, int wearerId)
        {
            var response = await _http.GetAsync($"api/Wearer/Next?custId={custId}&wearerId={wearerId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return NoContent();

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, error);
            }

            var model = await response.Content.ReadFromJsonAsync<WearerViewModel>();
            if (model == null) return NoContent();

            model.CustId = custId;
            return PartialView("Wearer", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveWearer([FromBody] UpdateWearerDTO dto)
        {
            if (dto == null) return BadRequest("Wearer data is missing.");

            var response = await _http.PostAsJsonAsync("api/Wearer", dto);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, responseBody);

            return Ok(responseBody);
        }

        public async Task<IActionResult> WearerSelection(int custId)
        {
            var wearers = await _http.GetFromJsonAsync<List<WearerViewModel>>($"api/Wearer/All?custId={custId}")
                          ?? [];
            return PartialView("_WearerSelection", wearers);
        }
    }
}