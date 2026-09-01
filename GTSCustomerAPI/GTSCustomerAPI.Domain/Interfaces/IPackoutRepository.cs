using GTSCustomerAPI.Domain.Entities;

namespace GTSCustomerAPI.Domain.Interfaces;

public interface IPackoutRepository
{
    Task<IEnumerable<PackoutDetails>>
        GetPackoutByCustIdAsync(int custId);
    Task<IEnumerable<string>>
        GetPackoutItemsAsync(int custId);
    Task<int> SavePackoutAsync(PackoutSave data);
    Task<int> DeletePackoutAsync(int restrictId);
}