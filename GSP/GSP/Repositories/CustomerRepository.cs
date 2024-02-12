using GSP.Data;
using GSP.DTO;
using GSP.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GSP.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            // Check if a customer with the same CustomerCode already exists
            var existingCustomer = await applicationDbContext.Customers
                                        .FirstOrDefaultAsync(c => c.CustomerCode == customer.CustomerCode);

            if (existingCustomer == null)
            {
                await applicationDbContext.Customers.AddAsync(customer);
                await applicationDbContext.SaveChangesAsync();
                return customer;
            } else
            {
                var cust = new Customer 
                { 
                    Id = existingCustomer.Id,
                    CustomerCode = existingCustomer.CustomerCode,
                    CustomerName = existingCustomer.CustomerName,
                    Phone = existingCustomer.Phone,
                    Address = existingCustomer.Address,
                    Email = existingCustomer.Email,
                };
                return cust;
            } 
           

        }

        public async Task<Customer?> DeleteCustomerAsync(int id)
        {
            var customer = await applicationDbContext.Customers.FindAsync(id);
            if (customer != null)
            {
                applicationDbContext.Customers.Remove(customer);
                await applicationDbContext.SaveChangesAsync();
                return customer;
            }
            else
            {
                return null;
            }

           
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customers = await applicationDbContext.Customers.ToListAsync();
            return customers;
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            var customer = await applicationDbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
            return customer;
        }

        public async Task<Customer?> UpdateCustomerAsync(Customer customer)
        {
            var customerToUpdate = await applicationDbContext.Customers.FindAsync(customer.Id);
            if (customerToUpdate != null)
            {
                customerToUpdate.CustomerName = customer.CustomerName;
                customerToUpdate.CivilId = customer.CivilId;
                customerToUpdate.Phone = customer.Phone;
                customerToUpdate.Email = customer.Email;
                customerToUpdate.Address = customer.Address;
                await applicationDbContext.SaveChangesAsync();
                return customerToUpdate;
            } else
            {
                return null;
            }
        }

        public async Task<int> GetCustomerCountAsync()
        {
            return await applicationDbContext.Customers.CountAsync();
        }

        public async Task<IEnumerable<Customer>> AddBulkCustomersAsync(IEnumerable<CustomerExcelDto> customers)
        {
            var existingCustomersIds = await applicationDbContext.Customers
                .ToListAsync();

            var newCustomers = customers
                .Where(dto => !existingCustomersIds.Any(cust => cust.CustomerCode == dto.CustomerCode && cust.CustomerName == dto.CustomerName))
                .Select(dto => new Customer
                {
                    CustomerCode = dto.CustomerCode,
                    CustomerName = dto.CustomerName,
                    CivilId = dto.CivilId,
                    Phone = dto.Phone,
                    Phone2 = dto.Phone2,
                    Phone3 = dto.Phone3,
                    Phone4 = dto.Phone4,
                    Email = dto.Email,
                    Address = dto.Address,
                });

            await applicationDbContext.Customers.AddRangeAsync(newCustomers);
            await applicationDbContext.SaveChangesAsync();

            return newCustomers;
        }
    }
}
