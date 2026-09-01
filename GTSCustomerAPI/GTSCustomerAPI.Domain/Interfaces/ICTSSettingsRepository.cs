using System;
using System.Collections.Generic;
using System.Text;

using GTSCustomerAPI.Domain.Entities;

namespace GTSCustomerAPI.Domain.Interfaces;

public interface ICTSSettingsRepository
{
    Task<CTSSettings?> GetByCustIdAsync(int custId);
    Task<bool> SaveAsync(CTSSettings settings);
}