using System;
using System.Collections.Generic;
using System.Text;
using GTSCustomerAPI.Application.DTOs;

namespace GTSCustomerAPI.Application.Interfaces;

public interface IDosageService
{
    Task<CustomerDosageDto?> GetDosageAsync(
        int custId, CancellationToken ct);
    Task UpdateDosageAsync(
        int custId, bool? dosage,
        CancellationToken ct);
}