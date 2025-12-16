using Microsoft.AspNetCore.Mvc;
using PickGo_backend.Models;
using PickGo_backend.DTOs.Customer;
using AutoMapper;

namespace PickGo_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Customer/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _unitOfWork.CustomerRepo.GetByIdAsync(id);
            if (customer == null) return NotFound();

            var dto = _mapper.Map<CustomerDto>(customer);
            return Ok(dto);
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _unitOfWork.CustomerRepo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return Ok(dtos);
        }
        // POST: api/Customer/join

        [HttpPost("join")]
        public async Task<IActionResult> JoinCustomer([FromBody] CustomerJoinDto dto)
        {

            if (string.IsNullOrEmpty(dto.PhoneNumber))
                return BadRequest("Phone number is required.");

            var customer = await _unitOfWork.CustomerRepo
                .GetByExpressionAsync(c => c.PhoneNumber == dto.PhoneNumber);

            if (customer == null)
            {
                // Create a new User
                var user = new User
                {
                    UserName = dto.UserName ?? dto.PhoneNumber, // use provided username or phone number
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.PhoneNumber + "@example.com",  // required by Identity
                    Address = dto.Address
                };

                await _unitOfWork.UserRepo.AddAsync(user);
                await _unitOfWork.SaveAsync();

                // Create Customer
                customer = new Customer
                {
                    PhoneNumber = dto.PhoneNumber,
                    UserId = user.Id
                };

                await _unitOfWork.CustomerRepo.AddAsync(customer);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                // Optional: update username/address if provided
                if (!string.IsNullOrEmpty(dto.UserName) && customer.User != null)
                    customer.User.UserName = dto.UserName;

                if (!string.IsNullOrEmpty(dto.Address) && customer.User != null)
                    customer.User.Address = dto.Address;

                await _unitOfWork.SaveAsync();
            }

            var result = _mapper.Map<CustomerDto>(customer);
            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerUpdateDto dto)
        {
            var customer = await _unitOfWork.CustomerRepo.GetByIdAsync(id);
            if (customer == null) return NotFound();

            _mapper.Map(dto, customer);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/Customer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _unitOfWork.CustomerRepo.GetByIdAsync(id);
            if (customer == null) return NotFound();

            _unitOfWork.CustomerRepo.Delete(customer); // use Delete, not Remove
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}