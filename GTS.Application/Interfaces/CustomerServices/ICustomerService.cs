using GTS.Application.DTOs;

namespace GTS.Application.Interfaces.CustomerServices
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO?> GetCustomerByIdAsync(int id);
        Task<UpdateCustomerDTO?> GetCustomerForUpdateAsync(int id);
        Task<CustomerDTO?> AddCustomerAsync(CreateCustomerDTO customerDto);
        Task UpdateCustomerAsync(UpdateCustomerDTO customerDto);
        Task DeleteCustomerAsync(int id);
    }
}