using GTS.Application.DTOs;

namespace GTS.Application.Interfaces
{
    public interface IPackoutService
    {
        Task<IEnumerable<PackoutDTO>>
            GetPackoutDetails(int custId);

        Task<IEnumerable<string>>
            GetPackoutItems(int custId);

        Task<int>
            SavePackout(PackoutDTO dto);

        Task<int>
            DeletePackout(int pkoutRestrictId);
    }
}