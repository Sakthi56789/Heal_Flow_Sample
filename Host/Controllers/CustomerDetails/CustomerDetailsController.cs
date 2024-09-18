using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.CustomerDetails;
using Domain.CustomerDetails;
using Microsoft.AspNetCore.Authorization;

namespace Host.Controllers.CustomerDetails
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerDetailsController : ControllerBase
    {
        private readonly ICustomer _customer;
        public CustomerDetailsController(ICustomer customer)
        {
            _customer = customer;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _customer.GetAll();
            return Ok(data);
        }
        [HttpGet]
        [Route("GetByID")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var data = await _customer.GetById(Id);
            if (data == null)
            {
                return NotFound("Data is Not there...!");
            }
            return Ok(data);
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Customer customerDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await _customer.Create(customerDetails);
            return Ok(data);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateCustomer(Guid Id, Customer customerDetails)
        {
            var data = await _customer.Update(Id, customerDetails);
            if (!data)
            {
                return NotFound("NotFound");
            }
            return Ok("Updated Successfull ....!");
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeletedCustomer(Guid Id)
        {
            var data = await _customer.Delete(Id);
            if (!data)
            {
                return NotFound("NotFound");
            }
            return Ok("Deleted Successfully...!");
        }
    }
}
