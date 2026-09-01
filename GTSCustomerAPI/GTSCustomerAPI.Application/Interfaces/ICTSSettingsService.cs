using System;
using System.Collections.Generic;
using System.Text;

using GTSCustomerAPI.Application.DTOs;

namespace GTSCustomerAPI.Application.Interfaces;

public interface ICTSSettingsService
{
    Task<CTSSettingsDto?> GetByCustIdAsync(int custId);
    Task<bool> SaveAsync(SaveCTSSettingsDto dto);
}