using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;

namespace GTSCustomerAPI.Application.Services;

public class CTSSettingsService : ICTSSettingsService
{
    private readonly ICTSSettingsRepository _repo;

    public CTSSettingsService(ICTSSettingsRepository repo)
    {
        _repo = repo;
    }

    public async Task<CTSSettingsDto?> GetByCustIdAsync(int custId)
    {
        var s = await _repo.GetByCustIdAsync(custId);
        if (s == null) return null;

        return new CTSSettingsDto
        {
            CustId = s.CustId,
            CustNbr = s.CustNbr,
            MarketCenter = s.MarketCenter,
            Name = s.Name,
            Route = s.Route,
            GID = s.GID,
            PrintIssueStatusFlag = s.PrintIssueStatusFlag,
            PrintBornonDateFlag = s.PrintBornonDateFlag,
            NOGFlag = s.NOGFlag,
            LabelHeader = s.LabelHeader
        };
    }

    public async Task<bool> SaveAsync(SaveCTSSettingsDto dto)
    {
        var entity = new CTSSettings
        {
            CustId = dto.CustId,
            PrintIssueStatusFlag = dto.PrintIssueStatusFlag,
            PrintBornonDateFlag = dto.PrintBornonDateFlag,
            NOGFlag = dto.NOGFlag,
            LabelHeader = dto.LabelHeader
        };
        return await _repo.SaveAsync(entity);
    }
}