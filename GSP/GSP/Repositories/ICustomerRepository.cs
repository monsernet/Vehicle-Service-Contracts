using GSP.DTO;
using GSP.Models.Domain;

namespace GSP.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer?> UpdateCustomerAsync(Customer customer);
        Task<Customer?> DeleteCustomerAsync(int id);
        Task<int> GetCustomerCountAsync();
        Task<IEnumerable<Customer>> AddBulkCustomersAsync(IEnumerable<CustomerExcelDto> customers);
    }
}
