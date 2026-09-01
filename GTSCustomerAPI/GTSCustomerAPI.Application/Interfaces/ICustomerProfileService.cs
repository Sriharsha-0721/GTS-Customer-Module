using GTSCustomerAPI.Application.DTOs;

namespace GTSCustomerAPI.Application.Interfaces;

public interface ICustomerProfileService
{
    Task<CustomerProfileDto?> GetByCustIdAsync(
        int custId);
    Task<bool> SaveAsync(UpdateCustomerProfileDto dto);
}