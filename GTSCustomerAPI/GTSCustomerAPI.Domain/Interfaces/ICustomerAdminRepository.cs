using System;
using System.Collections.Generic;
using System.Text;

using GTSCustomerAPI.Domain.Entities;

namespace GTSCustomerAPI.Domain.Interfaces;

public interface ICustomerAdminRepository
{
    Task<CustomerAdmin?> GetByCustIdAsync(int custId);
    Task<bool> SaveAsync(int custId, bool ossFlag, bool stFlag);
}