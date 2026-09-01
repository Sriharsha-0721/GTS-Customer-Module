
using GTSCustomerAPI.Application.DTOs;

namespace GTSCustomerAPI.Application.Interfaces;

public interface IWashService
{
    Task<IEnumerable<GarmentTypeDto>>
        GetGarmentTypesAsync(
            CancellationToken ct);

    Task<string>
        GetFormulaAsync(
            int marketCenter,
            int custId,
            CancellationToken ct);

    Task<int>
        SaveFormulaAsync(
            SaveFormulaDto dto,
            CancellationToken ct);
}