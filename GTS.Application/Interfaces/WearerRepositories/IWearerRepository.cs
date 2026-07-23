using GTS.Application.DTOs;
using GTS.Domain.Entities;

namespace GTS.Application.Interfaces.WearerRepositories
{
    public interface IWearerRepository
    {
        Task<Wearer?> GetWearerAsync(
            int custId,
            string wearNbr);

        Task<Wearer?> GetNextWearerAsync(
            int custId,
            int? wearerId);

        Task<Wearer?> GetPreviousWearerAsync(
            int custId,
            int? wearerId);

        Task<int> SaveWearerAsync(
            UpdateWearerDTO dto);
    }
}