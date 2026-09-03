using GTS.Application.DTOs;

namespace GTS.Application.Interfaces.CustomerRepositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomersSelListDto>> GetFilteredCustomersAsync(CustomerFilterRequestDto filter, CancellationToken ct = default);

        Task<CustomerFlagsDto?> GetCustomerFlagsAsync(int custId, CancellationToken ct = default);
        Task<int> SaveCustomerFlagsAsync(int custId, bool ossFlag, bool stfFlag, CancellationToken ct = default);

        Task<CustomerProfileDto?> GetCustomerProfileAsync(int custId, CancellationToken ct = default);
        Task<bool> UpdateCustomerProfileAsync(UpdateCustomerProfileDto dto, CancellationToken ct = default);
    }
}