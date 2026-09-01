using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;

namespace GTSCustomerAPI.Application.Services;

public class WearerService : IWearerService
{
    private readonly IWearerRepository _repo;

    public WearerService(IWearerRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<NextWearerDto>>
        GetAllWearers(int custId, CancellationToken ct)
    {
        var list = await _repo.GetAllWearers(custId);
        return list.Select(w => ToDto(w));
    }

    public async Task<IEnumerable<NextWearerDto>>
        NextWearerDetails(
        int custId, int? wrrId, CancellationToken ct)
    {
        var list = await _repo
            .GetNextWearer(custId, wrrId);
        return list.Select(w => ToDto(w));
    }

    public async Task<IEnumerable<NextWearerDto>>
        GetWearerDetails(
        int custId, string wrNbr, CancellationToken ct)
    {
        var list = await _repo.GetWearer(custId, wrNbr);
        return list.Select(w => ToDto(w));
    }

    public async Task<IEnumerable<NextWearerDto>>
        PrevWearerDetails(
        int custId, int? wrrId, CancellationToken ct)
    {
        var list = await _repo
            .GetPrevWearer(custId, wrrId);
        return list.Select(w => ToDto(w));
    }

    public async Task<int> SaveWearer(
        WearerDto dto, CancellationToken ct)
    {
        return await _repo.SaveWearer(new Wearer
        {
            WearerId = dto.WearerId,
            Locker = dto.Locker,
            LockRm = dto.LockRm,
            Sex = dto.IsMale ? "M" : "F"
        });
    }

    private static NextWearerDto ToDto(NextWearer w)
        => new()
        {
            WearerId = w.WearerId,
            CustId = w.CustId,
            CustNbr = w.CustNbr,
            WearNbr = w.WearNbr,
            FirstName = w.FirstName,
            LastName = w.LastName,
            Locker = w.Locker,
            LockRm = w.LockRm,
            SexInt = w.SexInt
        };
}