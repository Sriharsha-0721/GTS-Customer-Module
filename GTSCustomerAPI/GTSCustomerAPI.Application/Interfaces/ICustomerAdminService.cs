using System;
using System.Collections.Generic;
using System.Text;

using GTSCustomerAPI.Application.DTOs;

namespace GTSCustomerAPI.Application.Interfaces;

public interface ICustomerAdminService
{
    Task<CustomerAdminDto?> GetByCustIdAsync(int custId);
    Task<bool> SaveAsync(SaveCustomerAdminDto dto);
}
