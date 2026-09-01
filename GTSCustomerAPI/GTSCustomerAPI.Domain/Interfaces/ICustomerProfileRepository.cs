using GTSCustomerAPI.Domain.Entities;

namespace GTSCustomerAPI.Domain.Interfaces;

public interface ICustomerProfileRepository
{
    Task<CustomerProfile?> GetByCustIdAsync(int custId);
    Task<bool> SaveAsync(CustomerProfile profile,
        int userId);
}