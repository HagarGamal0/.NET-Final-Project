using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickGo_backend.DTOs.Customer;
using PickGo_backend.Models;

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
                    Email = dto.Email + "@example.com",  // required by Identity
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

        [HttpGet("MyOrders")]
        public async Task<IActionResult> MyOrders([FromQuery] string phoneNumber)
        {
            // Find customer by phone
            var customer = await _unitOfWork.CustomerRepo
                .GetByExpressionAsync(c => c.PhoneNumber == phoneNumber);
            if (customer == null) return NotFound("Customer not found");

            // Get all packages for this customer with related Request and Courier
            var packages = (await _unitOfWork.PackageRepo.GetAllAsync(include: p => p.Include(p => p.Request).Include(p => p.Courier)))
                .Where(p => p.CustomerID == customer.Id &&
                            (p.Status == PackageStatus.Pending ||
                             p.Status == PackageStatus.Assigned ||
                             p.Status == PackageStatus.OutForDelivery))
                .Select(p => new
                {
                    p.Id,
                    p.Status,
                    CODAmount = p.ShipmentCost, // or your COD field if exists
                    PickupLat = p.Request.PickupLat,
                    PickupLng = p.Request.PickupLng,
                    DropLat = p.Lat,
                    DropLng = p.Lang,
                    Courier = p.Courier != null ? new
                    {
                        p.Courier.Id,
                        p.Courier.VehicleType,
                        p.Courier.Rating
                    } : null
                })
                .ToList();

            return Ok(packages);


        }

        [HttpGet("{customerId}/packages")]
        public async Task<IActionResult> GetCustomerPackages(int customerId)
        {
            var customer = await _unitOfWork.CustomerRepo.GetByIdAsync(customerId);
            if (customer == null) return NotFound("Customer not found.");

            var packages = await _unitOfWork.PackageRepo.GetAllAsync(include: p => p.Include(p => p.Request).Include(p => p.Courier));

            var result = packages
                .Where(p => p.CustomerID == customerId &&
                       (p.Status == PackageStatus.Pending ||
                        p.Status == PackageStatus.Assigned ||
                        p.Status == PackageStatus.OutForDelivery))
                .Select(p => new
                {
                    p.Id,
                    p.Status,
                    PickupLat = p.Request.PickupLat,
                    PickupLng = p.Request.PickupLng,
                    DropLat = p.Lat,
                    DropLng = p.Lang,
                    p.Destination,
                    p.Description,
                    p.Weight,
                    p.Fragile,
                    Courier = p.Courier != null ? new
                    {
                        p.Courier.Id,
                        p.Courier.VehicleType,
                        p.Courier.Rating
                    } : null
                })
                .ToList();

            return Ok(result);
        }
        [HttpGet("{customerId}/packages/track")]

        // -------------------- UC-CUS-03: Track Package (Real-time) --------------------
        [HttpGet("TrackPackage/{packageId}")]
        public async Task<IActionResult> TrackPackage(int packageId)
        {
            // 1️⃣ Get package first
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
            if (package == null) return NotFound("Package not found.");

            // 2️⃣ Load related Request
            package.Request = await _unitOfWork.RequestRepo.GetByIdAsync(package.RequestID);

            // 3️⃣ Load related Courier (if assigned)
            if (package.CourierID.HasValue)
            {
                package.Courier = await _unitOfWork.CourierRepo.GetByIdAsync(package.CourierID.Value);
            }

            // 4️⃣ Return result
            return Ok(new
            {
                package.Id,
                package.Status,
                PickupLat = package.Request?.PickupLat,
                PickupLng = package.Request?.PickupLng,
                DropLat = package.Lat,
                DropLng = package.Lang,
                Courier = package.Courier != null ? new
                {
                    package.Courier.Id,
                    package.Courier.VehicleType,
                    package.Courier.Rating,
                    package.Courier.IsOnline
                } : null
            });
        }
        // -------------------- UC-CUS-04: Communicate / Add Notes --------------------
        [HttpPost("{packageId}/notes")]
        public async Task<IActionResult> AddDeliveryNotes(int packageId, [FromBody] CustomerPackageNoteDto dto)
        {
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
            if (package == null) return NotFound("Package not found.");

            package.ShipmentNotes = dto.Notes;
            _unitOfWork.PackageRepo.Update(package);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Notes updated successfully." });
        }

        // -------------------- UC-CUS-05: Rate Delivery --------------------
        [HttpPost("{packageId}/rate")]
        public async Task<IActionResult> RateDelivery(int packageId, [FromBody] CustomerRateDto dto)
        {
            // 1️⃣ Get package
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
            if (package == null) return NotFound("Package not found.");

            // 2️⃣ Load courier if assigned
            if (package.CourierID.HasValue)
            {
                package.Courier = await _unitOfWork.CourierRepo.GetByIdAsync(package.CourierID.Value);
            }

            // 3️⃣ Ensure package is delivered
            if (package.Status != PackageStatus.Delivered)
                return BadRequest("Package not delivered yet.");

            // 4️⃣ Update package rating
            package.ShipmentRating = dto.Rating;

            // 5️⃣ Update courier rating (you could calculate average if needed)
            if (package.Courier != null)
            {
                package.Courier.Rating = dto.Rating;
                _unitOfWork.CourierRepo.Update(package.Courier);
            }

            // 6️⃣ Save changes
            _unitOfWork.PackageRepo.Update(package);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Rating submitted successfully." });

        }
    }
}