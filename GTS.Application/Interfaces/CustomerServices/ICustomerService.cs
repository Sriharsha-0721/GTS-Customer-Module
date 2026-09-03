using GTS.Application.DTOs;

namespace GTS.Application.Interfaces.CustomerServices
{
    public interface ICustomerService
    {
        // Selection & Search SP (dbo.GTS_GetFilteredCustomers)
        Task<IEnumerable<CustomersSelListDto>> GetCustomersBasedOnFilter(CustomerFilterRequestDto filter, CancellationToken ct = default);

        // Submodule: Customer Flags
        Task<CustomerFlagsDto?> GetCustomerFlags(int custId, CancellationToken ct = default);
        Task<int> SaveCustomerFlags(int custId, bool ossFlag, bool stfFlag, CancellationToken ct = default);

        // Submodule: Customer Profile
        Task<CustomerProfileDto?> GetCustomerProfile(int custId, CancellationToken ct = default);
        Task<bool> UpdateCustomerProfile(UpdateCustomerProfileDto dto, CancellationToken ct = default);
    }
}