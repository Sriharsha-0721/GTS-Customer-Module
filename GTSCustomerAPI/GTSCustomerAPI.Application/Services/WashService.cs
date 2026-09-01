
using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using GTSCustomerAPI.Domain.Interfaces;

namespace GTSCustomerAPI.Application.Services;

public class WashService : IWashService
{
    private readonly IWashRepository _repo;

    public WashService(
        IWashRepository repo)
    {
        _repo = repo;
    }


    public async Task<IEnumerable<GarmentTypeDto>>
        GetGarmentTypesAsync(
            CancellationToken ct)
    {
        var data =
            await _repo
                .GetGarmentTypesAsync();


        return data.Select(
            x => new GarmentTypeDto
            {
                ItemCode =
                    x.ItemCode,

                ItemDesc =
                    x.ItemDesc,

                ShortDesc =
                    x.ShortDesc
            }
        );
    }


    public async Task<string>
        GetFormulaAsync(
            int marketCenter,
            int custId,
            CancellationToken ct)
    {
        return await _repo
            .GetFormulaAsync(
                marketCenter,
                custId);
    }


    public async Task<int>
        SaveFormulaAsync(
            SaveFormulaDto dto,
            CancellationToken ct)
    {
        return await _repo
            .SaveFormulaAsync(
                dto.MarketCenter,
                dto.CustId,
                dto.FormulaString);
    }
}