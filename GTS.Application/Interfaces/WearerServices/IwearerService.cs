using GTS.Application.DTOs;

namespace GTS.Application.Interfaces.WearerServices
{
    public interface IWearerService
    {
        Task<WearerDTO?> GetWearerAsync(
            int custId,
            string wearNbr);

        Task<WearerDTO?> GetNextWearerAsync(
            int custId,
            int? wearerId);

        Task<WearerDTO?> GetPreviousWearerAsync(
            int custId,
            int? wearerId);

        Task<int> SaveWearerAsync(
            UpdateWearerDTO dto);
    }
}