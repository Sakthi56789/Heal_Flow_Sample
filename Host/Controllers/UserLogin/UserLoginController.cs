using Application.UserLogin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Host.Controllers.UserLogin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IUser _user;
        private readonly IConfiguration _configuration;

        public UserLoginController(IUser user, IConfiguration configuration)
        {
            _user = user;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await _user.Login(login);
            if (data != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim ("Username",login.Username.ToString()),
                    new Claim ("Password",login.Password.ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: signIn);
                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { Token = tokenValue });
            }
            return Unauthorized("Username and Password Unmatched.. ... !");
        }



        [HttpPost("Registeration")]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if (register.Password != register.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await _user.Register(register);
            if (!data)
            {
                return Conflict("Username and Email ID Already Exists ....");
            }
            return Ok("User Registered Sucessfullty....! ");
        }


        [HttpGet("getAll")]
        public async Task<IActionResult> Getall()
        {
            var _data = await _user.GetAll();
            return Ok(_data);
        }
    }
}
