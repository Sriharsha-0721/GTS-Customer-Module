using GTSCustomerAPI.Application.DTOs;

namespace GTSCustomerAPI.Application.Interfaces;

public interface IWearerService
{
    Task<IEnumerable<NextWearerDto>> GetAllWearers(
        int custId, CancellationToken ct);
    Task<IEnumerable<NextWearerDto>> NextWearerDetails(
        int custId, int? wrrId, CancellationToken ct);
    Task<IEnumerable<NextWearerDto>> GetWearerDetails(
        int custId, string wrNbr, CancellationToken ct);
    Task<IEnumerable<NextWearerDto>> PrevWearerDetails(
        int custId, int? wrrId, CancellationToken ct);
    Task<int> SaveWearer(
        WearerDto wearer, CancellationToken ct);
}