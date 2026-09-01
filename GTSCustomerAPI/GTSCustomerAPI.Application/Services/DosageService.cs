using System;
using System.Collections.Generic;
using System.Text;

using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using GTSCustomerAPI.Domain.Interfaces;

namespace GTSCustomerAPI.Application.Services;

public class DosageService : IDosageService
{
    private readonly IDosageRepository _repo;

    public DosageService(IDosageRepository repo)
    {
        _repo = repo;
    }

    public async Task<CustomerDosageDto?> GetDosageAsync(
        int custId, CancellationToken ct)
    {
        var d = await _repo.GetDosageAsync(custId);
        if (d == null) return null;
        return new CustomerDosageDto
        {
            CustId = d.CustId,
            Dosage = d.Dosage
        };
    }

    public async Task UpdateDosageAsync(
        int custId, bool? dosage, CancellationToken ct)
    {
        await _repo.UpdateDosageAsync(custId, dosage);
    }
}