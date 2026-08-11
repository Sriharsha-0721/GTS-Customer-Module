using GTS.Domain.Entities;

namespace GTS.Domain.Interfaces
{
    public interface IPackoutRepository
    {
        Task<IEnumerable<PackoutDetails>>
            GetPackoutDetails(int custId);

        Task<IEnumerable<string>>
            GetPackoutItems(int custId);

        Task<int>
            SavePackout(CreatePackout dto);

        Task<int>
            DeletePackout(int pkoutRestrictId);
    }
}