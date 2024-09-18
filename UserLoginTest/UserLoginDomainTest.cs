using Domain.UserLogin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserLoginTest
{
    public class UserLoginDomainTest
    {

        private List<ValidationResult> ValidateModel(User user)
        {
            var res = new List<ValidationResult>();
            var context = new ValidationContext(user);
            Validator.TryValidateObject(user, context, res, true);
            return res;
        }

        [Fact]
        public void User_ShouldBeInValid_WhenUsernameTooLong()
        {
            var user = new User
            {
                Username = new string('A', 21),
                Password = "Password@123",
                Email = "user@gmail.com"
            };

            var validationresults = ValidateModel(user);
            Assert.Contains(validationresults, v => v.ErrorMessage.Contains("Maximum Accepts 20 letters only"));
        }

        [Fact]
        public void User_ShouldBeInvalid_WhenPasswordIsTooShort()
        {
            var user = new User
            {
                Username = "User",
                Password = "Pass1@",
                Email = "user@gmail.com"
            };

            var validationresults = ValidateModel(user);
            Assert.Contains(validationresults, v => v.ErrorMessage.Contains("Password must be at least 8 characters."));
        }

        [Fact]
        public void User_ShouldBeInvalid_WhenPasswordIsTooLong()
        {
            var user = new User
            {
                Username = "User",
                Password = new string('A', 21) + "1@#",
                Email = "user@gmail.com"
            };

            var validationresults = ValidateModel(user);
            Assert.Contains(validationresults, v => v.ErrorMessage.Contains("Password Cant be longer than 20 characters ."));
        }

        [Fact]
        public void User_ShouldBeInvalid_WhenPasswordNospecialCharacter()
        {
            var user = new User
            {
                Username = "User",
                Password = "Password123",
                Email = "user@gmail.com"
            };

            var validationresults = ValidateModel(user);
            Assert.Contains(validationresults, v => v.ErrorMessage.Contains("Password must have at least one uppercase letter, one lowercase letter, one digit, and one special character"));
        }

        [Fact]
        public void User_ShouldBeInvalid_WhenEmailisInvalid()
        {
            var user = new User
            {
                Username = "User",
                Password = "Password@123",
                Email = "user-gmail.com"
            };
            var validationresults = ValidateModel(user);
            Assert.Contains(validationresults, v => v.ErrorMessage.Contains("The Email field is not a valid e-mail address."));
        }

        [Fact]
        public void User_ShouldBeValid_WhenallFieldsareValid()
        {
            var user = new User
            {
                Username = "User",
                Password = "Password@123",
                Email = "user@gmail.com"
            };
            var validationresults = ValidateModel(user);
            Assert.Empty(validationresults);
        }

        [Fact]
        public void User_ShouldBeInvalid_WhenUsernameisMissing()
        {
            var user = new User
            {
                Password = "Password@123",
                Email = "user@gmail.com"
            };
            var validationresults = ValidateModel(user);
            Assert.Contains(validationresults, v => v.ErrorMessage.Contains("The Username field is required."));
        }

        [Fact]
        public void User_ShouldBeInvalid_WhenPasswordInValid()
        {
            var user = new User
            {
                Username = "User",
                Password = "password",
                Email = "test@example.com"
            };

            var validationresults = ValidateModel(user);
            Assert.Contains(validationresults, v => v.ErrorMessage.Contains("Password must have at least one uppercase letter, one lowercase letter, one digit, and one special character"));
        }
    }
}

