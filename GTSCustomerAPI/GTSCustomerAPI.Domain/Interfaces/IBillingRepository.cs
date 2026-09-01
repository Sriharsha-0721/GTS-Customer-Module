using GTSCustomerAPI.Domain.Entities;

namespace GTSCustomerAPI.Domain.Interfaces;

public interface IBillingRepository
{
    Task<IEnumerable<ChargeDetails>>
        GetBillingbyCustIdAsync(int custId);
    Task<IEnumerable<BillingData>>
        GetBillingDataAsync();
    Task<int> SaveBillingAsync(BillingCharge charge);
    Task<int> DeleteBillingAsync(int billingChargeId);
}