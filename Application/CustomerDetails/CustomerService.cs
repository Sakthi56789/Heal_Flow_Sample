using Domain.CustomerDetails;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.CustomerDetails
{
    public class CustomerService : ICustomer
    {
        private readonly CustomerDetailsDbContext _dbcontext;
        public CustomerService(CustomerDetailsDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _dbcontext.customerdetails.ToListAsync();
        }

        public async Task<Customer> Create(Customer customerDetails)
        {
            customerDetails.CustomerID = Guid.NewGuid();
            var existingCustomer = await _dbcontext.customerdetails.FirstOrDefaultAsync(c => c.CustomerID == customerDetails.CustomerID);
            if (existingCustomer != null)
            {
                throw new InvalidOperationException("Customer already exists.");
            }
            var _newCustomer = new Customer
            {
                CustomerName = customerDetails.CustomerName,
                Phone = customerDetails.Phone,
                MobileNumber = customerDetails.MobileNumber,
                EmailAddress = customerDetails.EmailAddress,
                Address = customerDetails.Address,
                City =  customerDetails.City,
                District = customerDetails.District,
                State = customerDetails.State,
                Pincode = customerDetails.Pincode

            };
            var data = await _dbcontext.customerdetails.AddAsync(_newCustomer);
                await _dbcontext.SaveChangesAsync();
                return data.Entity;
            
        }
        public async Task<bool> Update(Guid Id, Customer customerDetails)
        {
            var data = await _dbcontext.customerdetails.FindAsync(Id);
            if (data == null)
            {
                return false;
            }

            data.CustomerName = customerDetails.CustomerName;
            data.Phone = customerDetails.Phone;
            data.MobileNumber = customerDetails.MobileNumber;
            data.EmailAddress = customerDetails.EmailAddress;
            data.Address = customerDetails.Address;
            data.City = customerDetails.City;
            data.State = customerDetails.State;
            data.District = customerDetails.District;


            await _dbcontext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> Delete(Guid Id)
        {
            var data = await _dbcontext.customerdetails.FindAsync(Id);
            if (data == null)
            {
                return false;
            }
            _dbcontext.customerdetails.Remove(data);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Customer> GetById(Guid Id)
        {
            return await _dbcontext.customerdetails.FindAsync(Id);

        }
    }
}
