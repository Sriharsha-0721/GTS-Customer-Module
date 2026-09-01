using GTSCustomerAPI.Application.DTOs;

namespace GTSCustomerAPI.Application.Interfaces;

public interface IPackoutService
{
    Task<IEnumerable<PackoutDetailsDto>>
        GetPackoutByCustIdAsync(
        int custId, CancellationToken ct);
    Task<IEnumerable<string>>
        GetPackoutItemsAsync(
        int custId, CancellationToken ct);
    Task<int> SavePackoutAsync(
        PackoutSaveDto dto, CancellationToken ct);
    Task<int> DeletePackoutAsync(
        int restrictId, CancellationToken ct);
}