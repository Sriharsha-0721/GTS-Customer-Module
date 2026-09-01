using GTSCustomerAPI.Domain.Entities;

namespace GTSCustomerAPI.Domain.Interfaces;

public interface IWearerRepository
{
    Task<IEnumerable<NextWearer>> GetAllWearers(int custId);
    Task<IEnumerable<NextWearer>> GetNextWearer(
        int custId, int? wrrId);
    Task<IEnumerable<NextWearer>> GetPrevWearer(
        int custId, int? wrrId);
    Task<IEnumerable<NextWearer>> GetWearer(
        int custId, string wrNbr);
    Task<int> SaveWearer(Wearer wearer);
}