using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace GTS.Application.Services
{
    public class SoilStationService(ISoilStationRepository repo) : ISoilStationService
    {
        public async Task<ReceiverDto?> CheckReceiverAsync(long receiverId) => await repo.GetReceiverDataAsync(receiverId);

        public async Task<ReceiverDto?> ValidateReceiverAsync(long receiverId) => await repo.GetReceiverDataAsync(receiverId);

        public async Task<bool> ValidateContainerAsync(string containerId) => await repo.CheckContainerAsync(containerId);

        public async Task<ScanResultDto> ProcessScanAsync(SoilStationScanRequest request)
        {
            var garment = await repo.GetGarmentInfoAsync(request.GarmentBarcode);
            if (garment == null)
            {
                return new ScanResultDto { Success = false, StatusCode = "NOT_FOUND", Message = "Garment not found on file." };
            }

            bool validDay = await repo.CheckPickUpDayAsync(garment.CustId, request.PickupDay);
            if (!validDay)
            {
                return new ScanResultDto { Success = false, StatusCode = "DAY_ERROR", Message = "Garment pickup day does not match customer schedule." };
            }

            int maxWashRes = await repo.CheckExceedsMaxWashAsync(garment.CustId, garment.ItemCode, garment.Garment);
            if (maxWashRes == 1)
            {
                return new ScanResultDto { Success = false, StatusCode = "MAX_WASH", Message = "Garment has exceeded maximum allowed wash cycles." };
            }

            string newScanCode = request.IsRepair ? "PM" : "PR";
            DateTime now = DateTime.Now;

            bool updated = await repo.UpdateGarminServAsync(
                garment.Garment,
                garment.ScanCode ?? "PR",
                garment.ScanDt ?? now,
                newScanCode,
                now,
                1,
                request.UserId,
                now
            );

            if (!updated)
            {
                return new ScanResultDto { Success = false, StatusCode = "UPDATE_FAILED", Message = "Failed to update garment service record." };
            }

            int scanLogId = await repo.LogScanAsync(
                garment.Garment,
                "SI",
                newScanCode,
                garment.CustId,
                garment.WearerId ?? 0,
                garment.WearItemId ?? 0,
                garment.ItemCode,
                garment.Size ?? string.Empty,
                garment.ServCode ?? "ST",
                "O",
                now.Date,
                1,
                request.UserId,
                request.ReceiverId,
                request.ContainerId,
                0
            );

            return new ScanResultDto
            {
                Success = true,
                StatusCode = "OK",
                Message = "Scan successful.",
                Garment = garment.Garment,
                ItemCode = garment.ItemCode,
                Size = garment.Size,
                ScanLogId = scanLogId
            };
        }

        public async Task<ScanResultDto> ProcessGarmentScanAsync(ScanGarmentRequestDto request) => await ProcessScanAsync(request);

        public async Task<bool> UndoScanAsync(string garmentBarcode, int userId)
        {
            var garment = await repo.GetGarmentInfoAsync(garmentBarcode);
            if (garment == null) return false;

            DateTime now = DateTime.Now;
            return await repo.UpdateGarminServAsync(
                garment.Garment,
                garment.ScanCode ?? "PR",
                garment.ScanDt ?? now,
                "UN",
                now,
                1,
                userId,
                now
            );
        }

        public async Task<bool> UndoScanAsync(long receiverId, string garmentBarcode, int userId)
        {
            // Reverses the garment scan for the specific receiver intake
            return await UndoScanAsync(garmentBarcode, userId);
        }

        public async Task<SpecialInstructionsDto?> GetSpecialInstructionsAsync(int receiverId)
        {
            if (receiverId <= 0) return null;
            return await repo.GetSpecialInstructionsAsync(receiverId);
        }
    }
}