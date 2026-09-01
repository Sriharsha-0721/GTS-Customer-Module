using GTSCustomerAPI.Application.DTOs;

namespace GTSCustomerAPI.Application.Interfaces;

public interface IBillingService
{
    Task<IEnumerable<ChargeDetailsDto>>
        GetBillingbyCustIdAsync(
        int custId, CancellationToken ct);
    Task<IEnumerable<BillingDataDto>>
        GetBillingDataAsync(CancellationToken ct);
    Task<int> SaveBillingAsync(
        BillingChargeDto dto, CancellationToken ct);
    Task<int> DeleteBillingAsync(
        int billingChargeId, CancellationToken ct);
}
