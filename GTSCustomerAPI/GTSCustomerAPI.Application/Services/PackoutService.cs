
﻿using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;

namespace GTSCustomerAPI.Application.Services;

public class PackoutService : IPackoutService
{
    private readonly IPackoutRepository _repo;

    public PackoutService(IPackoutRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<PackoutDetailsDto>>
        GetPackoutByCustIdAsync(
        int custId, CancellationToken ct)
    {
        var list = await _repo
            .GetPackoutByCustIdAsync(custId);
        return list.Select(p => new PackoutDetailsDto
        {
            PkoutRestrictId = p.PkoutRestrictId,
            CustId = custId,
            PkoutRestrict = p.PkoutRestrict,
            Item = p.Item,
            Color = p.Color,
            Size = p.Size
        });
    }

    public async Task<IEnumerable<string>>
        GetPackoutItemsAsync(
        int custId, CancellationToken ct)
    {
        return await _repo
            .GetPackoutItemsAsync(custId);
    }

    public async Task<int> SavePackoutAsync(
        PackoutSaveDto dto, CancellationToken ct)
    {
        var entity = new PackoutSave
        {
            CustId = dto.CustId,
            PackoutRestrict = dto.PkoutRestrict,
            // ✅ Friend uses "Item" → SP needs @ItemType
            ItemType = dto.Item,
            Color = dto.Color,
            Size = dto.Size,
            UserId = dto.UpdtUser
        };
        return await _repo.SavePackoutAsync(entity);
    }

    public async Task<int> DeletePackoutAsync(
        int restrictId, CancellationToken ct)
    {
        return await _repo
            .DeletePackoutAsync(restrictId);
    }
}
