using Domain;
using Domain.CustomerDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomerDetails
{
    public interface ICustomer
    {
        Task<IEnumerable<Customer>> GetAll();   
        Task<Customer> Create(Customer customerDetails);
        Task<bool> Update(Guid Id, Customer customerDetails);
        Task<bool> Delete(Guid Id);
        Task<Customer> GetById(Guid Id);
    }
}
