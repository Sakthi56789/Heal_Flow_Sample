using Domain.CustomerDetails;
using Domain.UserLogin;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserLogin
{
    public class RegisterService : IUser
    {
        private readonly CustomerDetailsDbContext _dbContext;
        public RegisterService(CustomerDetailsDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<bool> Register(RegisterDto register)
        {

            var _data = await _dbContext.users.FirstOrDefaultAsync(x => x.Username == register.Username || x.Email == register.Email);
            if (_data == null)
            {
                var _user = new User
                {
                    Username = register.Username,
                    Email = register.Email,
                    Password = register.Password,
                 
                };
                _dbContext.users.Add(_user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<IEnumerable<User>> GetAll()
        {
            return await _dbContext.users.ToListAsync();
        }


        public async Task<User> Login(LoginDto login)
        {
            var _data = await _dbContext.users.FirstOrDefaultAsync(x => x.Username == login.Username && x.Password == login.Password);
            if (_data == null)
            {
                return null;
            }
            return _data;
        }



    }


    
}
