using Domain.UserLogin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserLogin
{
    public interface IUser
    {
        Task <bool> Register (RegisterDto register);
        Task<IEnumerable<User>> GetAll();
        Task<User> Login(LoginDto login);
       
    }
}
