using GTS.Application.DTOs;
using GTS.Domain.Entities;
using GTS.MVC.Models;
using GTS.MVC.Models.Billing;
using GTS.MVC.Models.CTSSettings;
using GTS.MVC.Models.CustomerLineComments;
using GTS.MVC.Models.CustomerProfile;
using GTS.MVC.Models.MaximumWash;
using GTS.MVC.Models.Packout;
using GTS.MVC.Models.SpecialLines;
using Microsoft.AspNetCore.Mvc;
//using GTS.MVC.Models.SpecialLines;
using GTS.MVC.Models.Wash;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http.Json;

namespace GTS.MVC.Controllers
{
    public class CustomerProfileController : Controller
    {
        private readonly HttpClient _http;

        public CustomerProfileController()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7161/")
            };
        }

        public IActionResult Index()
        {
            ViewBag.ActiveTab = "";
            return View();
        }

        public IActionResult Blank()
        {
            return PartialView("_Blank");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerViewModel model)
        {
            var response = await _http.PutAsJsonAsync(
                "api/Customer",
                model);

            if (response.IsSuccessStatusCode)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomerProfile(
            [FromBody] SaveCustomerProfileViewModel model)
        {
            // Get the complete customer from the API
            var customer = await _http.GetFromJsonAsync<UpdateCustomerDTO>(
                $"api/Customer/{model.CustId}/edit");

            if (customer == null)
                return BadRequest("Customer not found.");

            // Update only the comment fields
            customer.BillingCom = model.BillingCom;
            customer.PackoutCom = model.PackoutCom;
            customer.WashCom = model.WashCom;
            customer.Formula = model.Formula;
            Console.WriteLine("MODEL FORMULA: " + model.Formula);
            Console.WriteLine("CUSTOMER FORMULA BEFORE PUT: " + customer.Formula);
            customer.SoilCom = model.SoilCom;
            customer.DryerCom = model.DryerCom;
            customer.ReceivingCom = model.ReceivingCom;
            customer.ShippingCom = model.ShippingCom;
            customer.DriverCom = model.DriverCom;
            customer.MendCom = model.MendCom;
            customer.QACom = model.QACom;
            customer.CustSrvCom = model.CustSrvCom;
            customer.OfficeCom = model.OfficeCom;
            customer.GenOfficeCom = model.GenOfficeCom;
            customer.MerControlCom = model.MerControlCom;
            customer.MainCleanRoomCom = model.MainCleanRoomCom;
            customer.QAInspCom = model.QAInspCom;
            customer.ProdCom = model.ProdCom;

            customer.UpdtTime = DateTime.Now;

            if (customer.UpdtUser == 0)
            {
                customer.UpdtUser = 1;
            }
            // Save using the existing API
            var response = await _http.PutAsJsonAsync(
                $"api/Customer/{customer.CustId}",
                customer);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }

            return Ok();
        }

        public async Task<IActionResult> MaximumWash(int custId)
        {
            List<MaximumWashViewModel> model = new();

            if (custId > 0)
            {
                model = await _http.GetFromJsonAsync<List<MaximumWashViewModel>>
                    ($"api/MaxWash/{custId}") ?? new();
            }

            return PartialView("_MaximumWashGrid", model);
        }

        public async Task<IActionResult> Profile(int custId)
        {
            CustomerViewModel model = new();

            if (custId > 0)
            {
                model = await _http.GetFromJsonAsync<CustomerViewModel>(
                    $"api/Customer/{custId}") ?? new CustomerViewModel();
            }

            return PartialView("_Profile", model);
        }



        [HttpPost]
        public async Task<IActionResult> AddMaximumWash([FromBody] MaximumWashViewModel model)
        {
            var dto = new
            {
                CustId = model.CustId,
                ItemCode = model.ItemCode,
                MaxWash = model.MaxWash,
                MaxWeeks = model.MaxWeeks,
                MaxCycles = model.MaxCycles
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
                CustId = model.CustId,
                OldItemCode = model.OldItemCode,
                NewItemCode = model.NewItemCode,
                MaxWash = model.MaxWash,
                MaxWeeks = model.MaxWeeks,
                MaxCycles = model.MaxCycles
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
            var response = await _http.DeleteAsync(
                $"api/MaxWash/{custId}/{itemCode}");

            var result = await response.Content.ReadAsStringAsync();

            return Ok(result);
        }

        public async Task<IActionResult> CTSSettings(int custId)
        {
            CTSSettingsViewModel model = new();

            if (custId <= 0)
                return PartialView("_CTSSettings", model);

            // Customer Details
            var customer = await _http.GetFromJsonAsync<CustomerViewModel>(
                $"api/Customer/{custId}");

            if (customer == null)
                return PartialView("_CTSSettings", model);


            model.MC = customer.MC;
            model.CustID = customer.CustId;
            model.CustNo = customer.CustNo;
            model.Customer = customer.Name;
            model.Route = customer.Route;
            model.GID = customer.Gid;


            var response = await _http.GetAsync($"api/CTSSetting/{customer.CustNo}");

            var json = await response.Content.ReadAsStringAsync();



            var data = System.Text.Json.JsonSerializer.Deserialize<List<CTSSettingDetailsDTO>>
            (
                json,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            var cts = data?.FirstOrDefault();

            if (cts != null)
            {


                model.ItemCode = cts.ItemCode;
                model.PrintIssueStatusFlag = cts.PrintIssueStatusFlag == 1;
                model.PrintBornonDateFlag = cts.PrintBornonDateFlag == 1;
                model.NOGFlag = cts.NOGFlag == 1;
                model.LabelHeader = cts.LabelHeader;
            }



            return PartialView("_CTSSettings", model);
        }

        public async Task<IActionResult> CustomerLineComments(int custId)
        {
            List<CustomerLineCommentsViewModel> model = new();

            if (custId > 0)
            {
                model = await _http.GetFromJsonAsync<List<CustomerLineCommentsViewModel>>
                (
                    $"api/CustomerLineComments/{custId}"
                ) ?? new();
            }

            return PartialView("_CustomerLineComments", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomerLineComments(
            [FromBody] CustomerLineCommentsViewModel model)
        {
            var dto = new
            {
                CustId = model.CustId,
                WearItemId = model.WearItemId,
                Descr = model.Descr
            };

            var response =
                await _http.PostAsJsonAsync(
                    "api/CustomerLineComments",
                    dto);

            var result =
                await response.Content.ReadAsStringAsync();

            return Ok(result);
        }

        public async Task<IActionResult> Customer(int custId)
        {
            var model = await _http.GetFromJsonAsync<CustomerViewModel>(
                $"api/Customer/{custId}");

            if (model == null)
                return NotFound();

            return PartialView("Customer", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomerFlags(
    [FromBody] CustomerViewModel model)
        {
            var customer = await _http.GetFromJsonAsync<UpdateCustomerDTO>(
                $"api/Customer/{model.CustId}/edit");

            if (customer == null)
                return BadRequest("Customer not found.");

            customer.OSSFlag = model.OSSFlag;
            customer.STFlag = model.STFlag;
            customer.UpdtTime = DateTime.Now;

            if (customer.UpdtUser == 0)
                customer.UpdtUser = 1;

            var response = await _http.PutAsJsonAsync(
                $"api/Customer/{customer.CustId}",
                customer);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Wearer(int custId)
        {
            if (custId <= 0)
                return BadRequest("Customer is required.");

            var response = await _http.GetAsync(
                $"api/Wearer/Next?custId={custId}&wearerId=");

            if (response.StatusCode ==
                System.Net.HttpStatusCode.NotFound)
            {
                return PartialView(
                    "Wearer",
                    new WearerViewModel
                    {
                        CustId = custId
                    });
            }

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                return StatusCode(
                    (int)response.StatusCode,
                    error);
            }

            var model =
                await response.Content
                    .ReadFromJsonAsync<WearerViewModel>();

            if (model == null)
            {
                model = new WearerViewModel();
            }

            model.CustId = custId;

            return PartialView("Wearer", model);
        }
        [HttpGet]
        public async Task<IActionResult> WearerPrevious(
           int custId,
           int wearerId)
        {
            var response = await _http.GetAsync(
                $"api/Wearer/Previous?custId={custId}&wearerId={wearerId}");

            if (response.StatusCode ==
                System.Net.HttpStatusCode.NotFound)
            {
                return NoContent();
            }

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                return StatusCode(
                    (int)response.StatusCode,
                    error);
            }

            var model =
                await response.Content
                    .ReadFromJsonAsync<WearerViewModel>();

            if (model == null)
                return NoContent();

            model.CustId = custId;

            return PartialView("Wearer", model);
        }
        [HttpGet]
        public async Task<IActionResult> WearerNext(
            int custId,
            int wearerId)
        {
            var response = await _http.GetAsync(
                $"api/Wearer/Next?custId={custId}&wearerId={wearerId}");

            if (response.StatusCode ==
                System.Net.HttpStatusCode.NotFound)
            {
                return NoContent();
            }

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                return StatusCode(
                    (int)response.StatusCode,
                    error);
            }

            var model =
                await response.Content
                    .ReadFromJsonAsync<WearerViewModel>();

            if (model == null)
                return NoContent();

            model.CustId = custId;

            return PartialView("Wearer", model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveWearer(
    [FromBody] UpdateWearerDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Wearer data is missing.");
            }

            Console.WriteLine("========== MVC SAVE WEARER ==========");
            Console.WriteLine($"WearerId: {dto.WearerId}");
            Console.WriteLine($"Locker: {dto.Locker}");
            Console.WriteLine($"LockRm: {dto.LockRm}");
            Console.WriteLine($"Sex: {dto.Sex}");
            Console.WriteLine("=====================================");

            var response = await _http.PostAsJsonAsync(
                "api/Wearer",
                dto);

            var responseBody =
                await response.Content.ReadAsStringAsync();

            Console.WriteLine(
                $"API Status: {response.StatusCode}");

            Console.WriteLine(
                $"API Response: {responseBody}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(
                    (int)response.StatusCode,
                    responseBody);
            }

            return Ok(responseBody);
        }
        public async Task<IActionResult> WearerSelection(int custId)
        {
            var wearers =
                await _http.GetFromJsonAsync<List<WearerViewModel>>(
                    $"api/Wearer/All?custId={custId}")
                ?? new List<WearerViewModel>();

            return PartialView("_WearerSelection", wearers);
        }
        public async Task<IActionResult> SpecialLines(
            int custId,
            int page = 1)
        {
            SpecialLinePageViewModel model = new();

            if (custId > 0)
            {
                model =
                    await _http.GetFromJsonAsync<SpecialLinePageViewModel>
                    (
                        $"api/SpecialLines/{custId}?page={page}&pageSize=14"
                    ) ?? new SpecialLinePageViewModel();

                foreach (var item in model.Items)
                {
                    item.IsSelected = item.CustId.HasValue;
                }
            }

            return PartialView("_SpecialLines", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddSpecialLine(int custId, int line)
        {
            var response =
                await _http.PostAsync(
                    $"api/SpecialLines/{custId}/{line}",
                    null);

            var result =
                await response.Content.ReadAsStringAsync();

            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteSpecialLine(int line)
        {
            var response =
                await _http.DeleteAsync(
                    $"api/SpecialLines/{line}");

            var result =
                await response.Content.ReadAsStringAsync();

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> SaveSpecialLines(
    [FromBody] SaveSpecialLinesViewModel model)
        {
            var response =
                await _http.PostAsJsonAsync(
                    "api/SpecialLines",
                    model);

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                return StatusCode(
                    (int)response.StatusCode,
                    error);
            }

            return Ok();
        }
        public async Task<IActionResult> Billing(int custId)
        {
            BillingViewModel model = new();

            if (custId > 0)
            {
                model.Charges =
                    await _http.GetFromJsonAsync<List<BillingChargeViewModel>>
                    ($"api/Billing/{custId}") ?? new();

                model.ChargeTypes =
                    await _http.GetFromJsonAsync<List<BillingDataViewModel>>
                    ("api/Billing/BillingData") ?? new();

                var customer =
                    await _http.GetFromJsonAsync<CustomerViewModel>
                    ($"api/Customer/{custId}") ?? new CustomerViewModel();

                model.BillingCom = customer.BillingCom;
            }

            return PartialView("_Billing", model);
        }
       [HttpPost]
        public async Task<IActionResult> SaveBilling([FromBody] BillingChargeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _http.PostAsJsonAsync("api/Billing", model);

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, result);

            return Content(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBilling(int billingChargesId)
        {
            var response =
                await _http.DeleteAsync($"api/Billing/{billingChargesId}");

            var result =
                await response.Content.ReadAsStringAsync();

            return Ok(result);
        }
        public async Task<IActionResult> Packout(int custId)
        {
            PackoutViewModel model = new();

            if (custId > 0)
            {
                model.PackoutDetails =
                    await _http.GetFromJsonAsync<List<PackoutDetailsViewModel>>
                    ($"api/Packout/{custId}") ?? new();

                var itemCodes =
                    await _http.GetFromJsonAsync<List<string>>
                    ($"api/Packout/Items/{custId}") ?? new();

                model.Items = itemCodes
                    .Select(x => new PackoutItemViewModel
                    {
                        Item = x
                    })
                    .ToList();

                var customer =
                    await _http.GetFromJsonAsync<CustomerViewModel>
                    ($"api/Customer/{custId}")
                    ?? new CustomerViewModel();

                model.PackoutCom = customer.PackoutCom;
            }

            return PartialView("_Packout", model);
        }
        [HttpPost]
        public async Task<IActionResult> SavePackout([FromBody] PackoutSaveViewModel model)
        {
            var response =
                await _http.PostAsJsonAsync("api/Packout", model);

            var result =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, result);

            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePackout(int pkoutRestrictId)
        {
            try
            {
                if (pkoutRestrictId <= 0)
                    return BadRequest("Invalid Packout ID.");

                var response =
                    await _http.DeleteAsync(
                        $"api/Packout/{pkoutRestrictId}");

                var result =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(
                        (int)response.StatusCode,
                        result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        public async Task<IActionResult> Wash(int custId)
        {
            WashViewModel model = new();

            if (custId > 0)
            {
                // Get Wash details
                var wash =
                    await _http.GetFromJsonAsync<WashDTO>
                    ($"api/Wash/{custId}")
                    ?? new WashDTO();

                model.CustId = wash.CustId;
                model.WashCom = wash.WashCom ?? "";

                // ---------------------------------------------
                // Parse Customer.Formula
                //
                // Example:
                // All,6,Shirt,8,PANT,5
                // ---------------------------------------------

                if (!string.IsNullOrWhiteSpace(wash.Formula))
                {
                    var values =
                        wash.Formula.Split(
                            ',',
                            StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0;
                         i + 1 < values.Length;
                         i += 2)
                    {
                        model.Formulas.Add(
                            new WashFormulaViewModel
                            {
                                GarmentType = values[i].Trim(),
                                Formula = values[i + 1].Trim()
                            });
                    }
                }

                // ---------------------------------------------
                // Get global Garment Types
                // ---------------------------------------------

                model.GarmentTypes =
                    await _http.GetFromJsonAsync<List<string>>
                    ("api/Wash/GarmentTypes")
                    ?? new();
            }

            return PartialView("_Wash", model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveWash(
            [FromBody] WashDTO model)
        {
            var response =
                await _http.PostAsJsonAsync(
                    "api/Wash",
                    model);

            var result =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode(
                    (int)response.StatusCode,
                    result);

            return Ok(result);
        }
    }
}