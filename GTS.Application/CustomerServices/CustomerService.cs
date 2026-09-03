using GTS.Application.DTOs;
using GTS.Application.Interfaces.CustomerRepositories;
using GTS.Application.Interfaces.CustomerServices;

namespace GTS.Application.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        // 1. Selection & Search
        public async Task<IEnumerable<CustomersSelListDto>> GetCustomersBasedOnFilter(CustomerFilterRequestDto filter, CancellationToken ct = default)
        {
            return await _repository.GetFilteredCustomersAsync(filter, ct);
        }

        // 2. Submodule: Customer Flags
        public async Task<CustomerFlagsDto?> GetCustomerFlags(int custId, CancellationToken ct = default)
        {
            return await _repository.GetCustomerFlagsAsync(custId, ct);
        }

        public async Task<int> SaveCustomerFlags(int custId, bool ossFlag, bool stfFlag, CancellationToken ct = default)
        {
            return await _repository.SaveCustomerFlagsAsync(custId, ossFlag, stfFlag, ct);
        }

        // 3. Submodule: Customer Profile
        public async Task<CustomerProfileDto?> GetCustomerProfile(int custId, CancellationToken ct = default)
        {
            return await _repository.GetCustomerProfileAsync(custId, ct);
        }

        public async Task<bool> UpdateCustomerProfile(UpdateCustomerProfileDto dto, CancellationToken ct = default)
        {
            return await _repository.UpdateCustomerProfileAsync(dto, ct);
        }
    }
}