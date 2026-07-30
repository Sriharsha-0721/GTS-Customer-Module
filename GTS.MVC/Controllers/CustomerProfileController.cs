using GTS.Application.DTOs;
using GTS.MVC.Models;
using GTS.MVC.Models.CTSSettings;
using GTS.MVC.Models.CustomerLineComments;
using GTS.MVC.Models.CustomerProfile;
using GTS.MVC.Models.MaximumWash;
using GTS.MVC.Models.SpecialLines;
using Microsoft.AspNetCore.Mvc;
//using GTS.MVC.Models.SpecialLines;
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
                model = await _http.GetFromJsonAsync<CustomerViewModel>
                    ($"api/Customer/{custId}") ?? new CustomerViewModel();
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

        public IActionResult Customer(int custId)
        {
            return PartialView("Customer",
                new CustomerViewModel
                {
                    CustId = custId
                });
        }

        public IActionResult Wearer(int custId)
        {
            return PartialView("Wearer",
                new WearerViewModel
                {
                    CustId = custId
                });
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
    }
}