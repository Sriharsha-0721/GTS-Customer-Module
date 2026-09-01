using System;
using System.Collections.Generic;
using System.Text;

using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using GTSCustomerAPI.Domain.Interfaces;

namespace GTSCustomerAPI.Application.Services;

public class CustomerAdminService : ICustomerAdminService
{
    private readonly ICustomerAdminRepository _repo;

    public CustomerAdminService(ICustomerAdminRepository repo)
    {
        _repo = repo;
    }

    public async Task<CustomerAdminDto?> GetByCustIdAsync(
        int custId)
    {
        var ca = await _repo.GetByCustIdAsync(custId);
        if (ca == null) return null;
        return new CustomerAdminDto
        {
            CustId = ca.CustId,
            OSSFlag = ca.OSSFlag,
            STFlag = ca.STFlag
        };
    }

    public async Task<bool> SaveAsync(SaveCustomerAdminDto dto)
    {
        return await _repo.SaveAsync(
            dto.CustId, dto.OSSFlag, dto.STFlag);
    }
}