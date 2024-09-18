using Domain.CustomerDetails;
using Domain.UserLogin;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure
{
    public class CustomerDetailsDbContext : DbContext
    {
        public CustomerDetailsDbContext(DbContextOptions<CustomerDetailsDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> customerdetails { get; set; }
        public DbSet<User> users { get; set; }
  
    }
}
