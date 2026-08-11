using GTS.Domain.Entities;

namespace GTS.Domain.Interfaces
{
    public interface IWashRepository
    {
        Task<WashDetails?> GetWashDetails(int custId);

        Task<IEnumerable<string>> GetGarmentTypes();

        Task<int> SaveWash(WashDetails dto);
    }
}