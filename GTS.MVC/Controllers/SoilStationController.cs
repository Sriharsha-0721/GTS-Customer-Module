using GTS.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace GTS.MVC.Controllers
{
    public class SoilStationController(IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly HttpClient _apiClient = httpClientFactory.CreateClient("GtsApiClient");

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> ValidateReceiver(long receiverId)
        {
            try
            {
                var response = await _apiClient.GetAsync($"api/SoilStation/Receiver/{receiverId}");
                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Receiver not found or invalid." });
                }

                var data = await response.Content.ReadFromJsonAsync<ReceiverDto>();
                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"API Connection Error: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ValidateContainer(string containerId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(containerId))
                {
                    return Json(new { success = false, message = "Container barcode is required." });
                }

                // Post to the backend API
                var response = await _apiClient.PostAsJsonAsync("api/SoilStation/ValidateContainer", containerId);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Container Valid" });
                }

                return Json(new { success = false, message = "Container cannot be found. Please try another." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"API Connection Error: {ex.Message}" });
            }
        }   

        [HttpPost]
        public async Task<IActionResult> ScanGarment([FromBody] SoilStationScanRequest request)
        {
            try
            {
                var response = await _apiClient.PostAsJsonAsync("api/SoilStation/Scan", request);
                var result = await response.Content.ReadFromJsonAsync<ScanResultDto>();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"API Connection Error: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UndoScan([FromBody] SoilStationScanRequest request)
        {
            try
            {
                var response = await _apiClient.PostAsJsonAsync("api/SoilStation/Undo", request);
                var content = await response.Content.ReadFromJsonAsync<dynamic>();

                return Json(new { success = response.IsSuccessStatusCode, message = content?.message?.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"API Connection Error: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecialInstructions(int receiverId)
        {
            try
            {
                var response = await _apiClient.GetAsync($"api/SoilStation/SpecialInstructions/{receiverId}");
                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "No special instructions available." });
                }

                var content = await response.Content.ReadFromJsonAsync<dynamic>();
                return Json(content);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"API Connection Error: {ex.Message}" });
            }
        }
    }
}