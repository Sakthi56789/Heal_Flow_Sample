using Domain.CustomerDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerDetailsTest
{
    public class CustomerDetailsDomainTest
    {
        private List<ValidationResult> ValidateModel(Customer customer)
        {
            var res = new List<ValidationResult>();
            var context = new ValidationContext(customer);
            Validator.TryValidateObject(customer, context, res, true);
            return res;
        }

        [Fact]
        public void Customer_ShouldBeValid_WhenAllFieldsAreValid()
        {
            var customer = new Customer
            {
                CustomerID = Guid.NewGuid(),
                CustomerName = "John Doe",
                MobileNumber = "1234567890",
                EmailAddress = "user.doe@example.com",
                City = "New York",
                State = "New York",
                Pincode = "123456"
            };

            var result = ValidateModel(customer);

            Assert.Empty(result); 
        }

        [Fact]
        public void Customer_ShouldBeInValid_WhenCustomerNameMaxLength()
        {
            var customer = new Customer
            {
                CustomerID = Guid.NewGuid(),
                CustomerName = new string ('A',51)
            };
            var result = ValidateModel(customer);
            Assert.Contains(result, x =>x.MemberNames.Contains(nameof(Customer.CustomerName))&&
                                x.ErrorMessage.Contains("Maximum Accepts 20 letters only "));
        }
        [Fact]
        public void Customer_ShouldBeInvalid_WhenMobileNumberIsTooShort()
        {
            var customer = new Customer
            {
                CustomerID = Guid.NewGuid(),
                CustomerName = "FirstCustomer",
                MobileNumber = "123", 
                EmailAddress = "user@gmail.com",
                City = "NewCity",
                Pincode = "123456"
              
            };

            var result = ValidateModel(customer);

            Assert.Contains(result, x => x.MemberNames.Contains(nameof(Customer.MobileNumber)));
                                               }

        [Fact]
        public void Customer_ShouldBeInvalid_WhenEmailAddressIsInvalid()
        {
            var customer = new Customer
            {
                CustomerID = Guid.NewGuid(),
                CustomerName = "FirstCustomer",
                MobileNumber = "1234567890",
                EmailAddress = "user.example.com", 
            };
            var result = ValidateModel(customer);
            Assert.Contains(result, x => x.MemberNames.Contains(nameof(Customer.EmailAddress)) &&
                                        x.ErrorMessage.Contains("Accept the @ value Only"));
        }

        [Fact]
        public void Customer_ShouldBeInvalid_WhenPincodeIsNotcorrectDigits()
        {
            var customer = new Customer
            {
                CustomerID = Guid.NewGuid(),
                CustomerName = "John Doe",
                MobileNumber = "1234567890",
                EmailAddress = "john.doe@example.com",
                City = "NewCity",
                State = "TamilNadu",
                Pincode = "1234" 
            };

            var result = ValidateModel(customer);
            Assert.Contains(result, x => x.MemberNames.Contains(nameof(Customer.Pincode)) &&
                                        x.ErrorMessage.Contains("Only have 6 numbers"));
        }
        [Fact]
        public void Customer_ShouldBeInvalid_WhenStateSpecialCharacters()
        {
            var customer = new Customer
            {
                CustomerID = Guid.NewGuid(),
                CustomerName = "John Doe",
                MobileNumber = "1234567890",
                EmailAddress = "john.doe@example.com",
                City = "NewCity",
                State = "dsd@$", 
                Pincode = "123456"
            };

            var result = ValidateModel(customer);

            Assert.Contains(result, x => x.MemberNames.Contains(nameof(Customer.State)));
                                       
        }

        [Fact]
        public void Customer_ShouldBeInvalid_WhenRequiredFieldsAreMissing()
        {
            var customer = new Customer(); 
            var result = ValidateModel(customer);
            Assert.Contains(result, x => x.MemberNames.Contains(nameof(Customer.CustomerName)));
            Assert.Contains(result, x =>x.MemberNames.Contains(nameof(Customer.MobileNumber)));
            Assert.Contains(result, x => x.MemberNames.Contains(nameof(Customer.EmailAddress)));
            Assert.Contains(result, x => x.MemberNames.Contains(nameof(Customer.City)));
            Assert.Contains(result, x => x.MemberNames.Contains(nameof(Customer.State)));
            Assert.Contains(result, x =>x.MemberNames.Contains(nameof(Customer.Pincode)));
        }



    }
}
