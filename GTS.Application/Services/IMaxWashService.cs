using GTS.Application.DTOs;

namespace GTS.Application.Interfaces
{
    public interface IMaxWashService
    {
        Task<IEnumerable<MaxWashDTO>>
            GetMaxWash(int custId);

        Task<int>
            CreateMaxWash(MaxWashDTO dto);

        Task<int>
            UpdateMaxWash(MaxWashDTO dto);

        Task<int>
            DeleteMaxWash(
                int custId,
                string itemCode);
    }
}