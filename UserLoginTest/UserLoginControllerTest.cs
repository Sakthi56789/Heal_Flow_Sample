using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Application.UserLogin;
using Microsoft.Extensions.Configuration;
using Host.Controllers.UserLogin;
using Microsoft.AspNetCore.Mvc;
using Domain.UserLogin;

namespace UserLoginTest
{
    public class UserLoginControllerTest

    {
        private readonly Mock<IUser> _mockuserservice;
        private readonly Mock<IConfiguration> _mockconfiguration;
        private readonly UserLoginController _controller;

        public UserLoginControllerTest()
        {
            _mockuserservice = new Mock<IUser>();
            _mockconfiguration = new Mock<IConfiguration>();
            _controller = new UserLoginController(_mockuserservice.Object, _mockconfiguration.Object);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WithValid()
        {
            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "Password@123"
            };

            _mockuserservice.Setup(s => s.Login(loginDto))
                .ReturnsAsync(new User { Username = "testuser" });

            _mockconfiguration.Setup(c => c["Jwt:Key"]).Returns("SecretKeyHaveGiveAtLeast128BytesOnlyofAuthentication");
            _mockconfiguration.Setup(c => c["Jwt:Issuer"]).Returns("Issuer");
            _mockconfiguration.Setup(c => c["Jwt:Audience"]).Returns("Audience");
            _mockconfiguration.Setup(c => c["Jwt:Subject"]).Returns("Subject");
            var result = await _controller.Login(loginDto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenisInvalid()
        {
            var loginDto = new LoginDto
            {
                Username = "user",
                Password = "Password"
            };

            _mockuserservice.Setup(s => s.Login(loginDto))
                .ReturnsAsync((User)null);
            var result = await _controller.Login(loginDto) as UnauthorizedObjectResult;

            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("Username and Password Unmatched.. ... !", result.Value);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenRegisterIsValid()
        {
            var registerDto = new RegisterDto
            {
                Username = "newuser",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                Email = "user@gmail.com"
            };

            _mockuserservice.Setup(s => s.Register(registerDto))
                .ReturnsAsync(true);
            var result = await _controller.Register(registerDto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("User Registered Sucessfullty....! ", result.Value);
        }

        [Fact]
        public async Task Register_ShouldReturnError_WhenUsernameExists()
        {

            var registerDto = new RegisterDto
            {
                Username = "olduser",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                Email = "user@gmail.com"
            };

            _mockuserservice.Setup(s => s.Register(registerDto))
                .ReturnsAsync(false);

            var result = await _controller.Register(registerDto) as ConflictObjectResult;

            Assert.NotNull(result);
            Assert.Equal(409, result.StatusCode);
            Assert.Equal("Username and Email ID Already Exists ....", result.Value);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenPasswordDonotMatch()
        {
            var registerDto = new RegisterDto
            {
                Username = "user",
                Password = "Password@123",
                ConfirmPassword = "PAssword@113334",
                Email = "user@gmail.com"
            };
            var result = await _controller.Register(registerDto) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Passwords do not match.", result.Value);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfUsers()
        {

            var userList = new List<User>
            {
                new User { Username = "user1", Email = "user1@gmail.com" },
                new User { Username = "user2", Email = "user2@gmail.com" }
            };

            _mockuserservice.Setup(s => s.GetAll())
                .ReturnsAsync(userList);

            var result = await _controller.Getall() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<List<User>>(result.Value);

        }
    }
}

