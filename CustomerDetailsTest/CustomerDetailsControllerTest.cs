using Application.CustomerDetails;
using Domain.CustomerDetails;
using Host.Controllers.CustomerDetails;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerDetailsTest
{
    public class CustomerDetailsControllerTest
    {
        private readonly CustomerDetailsController _controller;
        private readonly Mock<ICustomer> _mockcustomerservice;

        public CustomerDetailsControllerTest()
        {
            _mockcustomerservice = new Mock<ICustomer>();
            _controller = new CustomerDetailsController(_mockcustomerservice.Object);
        }
        [Fact]
        public async Task GetAll_ShouldOkResult_WiththeCustomers()
        {
            var customers = new List<Customer>
        {
            new Customer { CustomerID = Guid.NewGuid(), CustomerName = "FirstCustomer" },
            new Customer { CustomerID = Guid.NewGuid(), CustomerName = "SecondCustomer" }
        };
            _mockcustomerservice.Setup(x => x.GetAll()).ReturnsAsync(customers);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCustomers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okResult.Value);
            Assert.Equal(customers.Count, returnCustomers.Count());
        }

        [Fact]
        public async Task GetById_ShouldOkResult_WhenCustomerExists()
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer { CustomerID = customerId, CustomerName = "Firstcustomer" };
            _mockcustomerservice.Setup(x => x.GetById(customerId)).ReturnsAsync(customer);

            var result = await _controller.GetById(customerId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCustomer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(customerId, returnCustomer.CustomerID);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            var customerId = Guid.NewGuid();
            _mockcustomerservice.Setup(x => x.GetById(customerId)).ReturnsAsync((Customer)null);

            var result = await _controller.GetById(customerId);

            Assert.IsType<NotFoundObjectResult>(result);
        }
        [Fact]
        public async Task Create_ShouldReturnOk_WhenModelIsValid()
        {
            var customer = new Customer 
            {
                CustomerID = Guid.NewGuid(),
                CustomerName = "Focuslogiccustomer"
            };
            _mockcustomerservice.Setup(x => x.Create(customer)).ReturnsAsync(customer);
            var result = await _controller.Create(customer);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCustomer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(customer.CustomerID, returnCustomer.CustomerID);
        }
        [Fact]
        public async Task Create_ShouldReturn_BadRequest_WhenModelStateInValid()
        {
            _controller.ModelState.AddModelError("Error", "Model state is invalid");
            var result = await _controller.Create(new  Customer());
            Assert.IsType<BadRequestObjectResult>(result);
        }
       
        [Fact]
        public async Task UpdateCustomer_ShouldReturnOk_WhenIsSuccessful()
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                CustomerID = customerId,
                CustomerName = "FirstCustomer"
            };
            _mockcustomerservice.Setup(x => x.Update(customerId, customer)).ReturnsAsync(true);
            var result = await _controller.UpdateCustomer(customerId, customer);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task UpdateCustomer_ShouldNotFound_WhenCustomerDoesnotExist()
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                CustomerID = customerId,
                CustomerName = "FirstCustomer"
            };
            _mockcustomerservice.Setup(x => x.Update(customerId, customer)).ReturnsAsync(false);
            var result = await _controller.UpdateCustomer(customerId, customer);
            Assert.IsType<NotFoundObjectResult>(result);
        }
        [Fact]
        public async Task DeletedCustomer_ShouldOk_WhenIsSuccessful()
        {
            var customerId = Guid.NewGuid();
            _mockcustomerservice.Setup(x => x.Delete(customerId)).ReturnsAsync(true);
            var result = await _controller.DeletedCustomer(customerId);
            Assert.IsType<OkObjectResult>(result);

        }
        [Fact]
        public async Task DeletedCustomer_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
           
            var customerId = Guid.NewGuid();
            _mockcustomerservice.Setup(x => x.Delete(customerId)).ReturnsAsync(false);
         
            var result = await _controller.DeletedCustomer(customerId);
           
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
