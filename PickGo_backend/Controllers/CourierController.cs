using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickGo_backend.DTOs.Courier;
using PickGo_backend.DTOs.DeliveryProof;
using PickGo_backend.Helpers;
using PickGo_backend.Models;
using PickGo_backend.Models.Enums;
using PickGo_backend.Services;
using System.Linq;
using System.Security.Claims;


namespace PickGo_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourierController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IGraphHopperService _graphHopper;
        private readonly IEmailService _emailService;

        public CourierController(UnitOfWork unitOfWork, IGraphHopperService graphHopper , IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _graphHopper = graphHopper;
            _emailService = emailService;

        }

        // -------------------- Get Online Couriers --------------------
        [HttpGet("Online")]
        public async Task<IActionResult> GetOnlineCouriers()
        {
            var couriers = (await _unitOfWork.CourierRepo.GetAllAsync())
                .Where(c => c.IsOnline && c.Status == CourierStatus.Approved)
                .Select(c => new
                {
                    c.Id,
                    c.UserId,
                    VehicleType = c.VehicleType.ToString(),
                    c.Rating,
                    c.MaxWeight
                })
                .ToList();

            return Ok(couriers);
        }

        // -------------------- Add Courier Location --------------------
        [HttpPost("AddLocation")]
        public async Task<IActionResult> AddLocation(
            [FromBody] CourierLocationDto request,
            [FromServices] IMapper mapper)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var courier = (await _unitOfWork.CourierRepo.GetAllAsync())
                          .FirstOrDefault(c => c.UserId == userId);
            if (courier == null) return NotFound("Courier profile not found.");

            // Map DTO to Model using AutoMapper
            var location = mapper.Map<CourierLocation>(request);
            location.CourierID = courier.Id;

            await _unitOfWork.CourierLocationRepo.AddAsync(location);
            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Location added successfully",
                location.Lat,
                location.Lng
            });
        }

        // -------------------- Toggle Online Status --------------------
        [HttpPost("ToggleOnlineStatus")]
        public async Task<IActionResult> ToggleOnlineStatus()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var courier = await _unitOfWork.CourierRepo.GetByExpressionAsync(c => c.UserId == userId);
            if (courier == null) return NotFound("Courier profile not found.");
            if (courier.Status != CourierStatus.Approved) return BadRequest("Courier is not approved yet.");

            courier.IsOnline = !courier.IsOnline;
            _unitOfWork.CourierRepo.Update(courier);
            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = courier.IsOnline ? "You are now online." : "You are now offline.",
                isOnline = courier.IsOnline
            });
        }

        // -------------------- Match Courier --------------------


        [HttpGet("MyAssignedPackages")]
        public async Task<IActionResult> MyAssignedPackages()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courier = await _unitOfWork.CourierRepo.GetByExpressionAsync(c => c.UserId == userId);
            if (courier == null) return NotFound("Courier not found");

            var packages = (await _unitOfWork.PackageRepo.GetAllAsync())
                .Where(p => p.CourierID == courier.Id &&
                           (p.Status == PackageStatus.Assigned ||
                            p.Status == PackageStatus.OutForDelivery))
                .Select(p => new
                {
                    p.Id,
                    Status = p.Status.ToString(),
                    CODAmount = p.ShipmentCost,
                    DestinationLat = p.Lat,
                    DestinationLng = p.Lang
                })
                .ToList();

            return Ok(packages);
        }

        //====================================== AcceptPackage ===========================
        [HttpPost("AcceptPackage/{packageId}")]
        public async Task<IActionResult> AcceptPackage(int packageId)
        {
            var courier = await GetCourierFromToken();
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);

            if (package == null || package.CourierID != courier.Id)
                return NotFound();

            package.Status = PackageStatus.Assigned;
            _unitOfWork.PackageRepo.Update(package);
            await _unitOfWork.SaveAsync();

            return Ok("Package accepted");
        }

        //=========================================================================

        [HttpPost("RejectPackage/{packageId}")]
        public async Task<IActionResult> RejectPackage(int packageId, [FromBody] string? reason)
        {
            var courier = await GetCourierFromToken();
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);

            if (package == null || package.CourierID != courier.Id)
                return NotFound();

            package.CourierID = null;
            package.Status = PackageStatus.Pending;

            _unitOfWork.PackageRepo.Update(package);
            await _unitOfWork.SaveAsync();

            return Ok("Package rejected");
        }


        private async Task<Courier> GetCourierFromToken()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Invalid token");

            var courier = await _unitOfWork.CourierRepo
                .GetByExpressionAsync(c => c.UserId == userId);

            if (courier == null)
                throw new Exception("Courier profile not found");

            return courier;
        }

        //==========================================================================

        [HttpPost("UpdateStatus/{packageId}")]
        public async Task<IActionResult> UpdateStatus(int packageId, PackageStatus status)
        {
            var courier = await GetCourierFromToken();
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);

            if (package == null || package.CourierID != courier.Id)
                return NotFound();

            package.Status = status;
            _unitOfWork.PackageRepo.Update(package);
            await _unitOfWork.SaveAsync();

            return Ok("Status updated");
        }

        //===================================================================

        [HttpPost("DeliverPackage/{packageId}")]
        public async Task<IActionResult> DeliverPackage(int packageId, [FromBody] DeliverPackageDto dto)
        {
            var courier = await GetCourierFromToken();
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
            if (package == null) return NotFound("Package not found");
            if (package.CourierID != courier.Id) return Forbid();
            if (package.Status != PackageStatus.OutForDelivery) return BadRequest("Package not out for delivery");

            var proof = new DeliveryProof
            {
                PackageID = package.Id,
                CourierID = courier.Id,
                CustomerID = package.CustomerID,
                DeliveredAt = DateTime.UtcNow,
                Notes = dto.Notes
            };

            await _unitOfWork.DeliveryProofRepo.AddAsync(proof);
            _unitOfWork.PackageRepo.Update(package);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Delivery proof recorded, waiting OTP" });
        }


        //=========================================================================

        [HttpPost("FailDelivery/{packageId}")]
        public async Task<IActionResult> FailDelivery(int packageId, [FromBody] string reason)
        {
            var courier = await GetCourierFromToken();
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);

            if (package == null || package.CourierID != courier.Id)
                return NotFound();

            package.Status = PackageStatus.Failed;
            _unitOfWork.PackageRepo.Update(package);
            await _unitOfWork.SaveAsync();

            return Ok("Delivery failed");
        }

        //==================================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourier(int id)
        {
            var courier = await _unitOfWork.CourierRepo.GetByIdWithIncludesAsync(id);
            if (courier == null) return NotFound();

            // Nullify dependent FKs
            foreach (var cs in courier.CourierSubscriptions)
                cs.CourierId = 0;

            if (courier.CurrentSubscriptionId != null)
                courier.CurrentSubscriptionId = null;

            _unitOfWork.CourierRepo.Delete(courier);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Courier deleted safely" });
        }


        private string GenerateOTP()
        {
            return Random.Shared.Next(100000, 999999).ToString();
        }

        //=======================================================================
        [HttpPost("StartDelivery/{packageId}")]
        public async Task<IActionResult> StartDelivery(int packageId)
        {
            var courier = await GetCourierFromToken();
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
            if (package == null) return NotFound("Package not found");
            if (package.CourierID != courier.Id) return Forbid();
            if (package.Status != PackageStatus.Assigned) return BadRequest("Package not ready");

            package.Status = PackageStatus.OutForDelivery;
            package.DeliveryOTP = Random.Shared.Next(100000, 999999).ToString();
            package.OTPVerified = false;

            _unitOfWork.PackageRepo.Update(package);
            await _unitOfWork.SaveAsync();

            await _emailService.SendEmailAsync(
                package.Customer.User.Email,
                "Your OTP for package delivery",
                $"Your OTP: {package.DeliveryOTP}"
            );

            return Ok(new { message = "Delivery started", note = "OTP sent to customer" });
        }


        //=====================================================================

        [HttpPost("VerifyOTP/{packageId}")]
        public async Task<IActionResult> VerifyOTP(int packageId, [FromBody] string otp)
        {
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
            if (package == null) return NotFound("Package not found");

            if (package.DeliveryOTP != otp) return BadRequest("Invalid OTP");

            package.OTPVerified = true;
            package.Status = PackageStatus.Delivered;
            package.DeliveredAt = DateTime.UtcNow;

            _unitOfWork.PackageRepo.Update(package);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Package delivered successfully" });
        }



        [HttpGet("AvailableJobs")]
        public async Task<IActionResult> GetAvailableJobs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // جلب بيانات الكورير
            var courier = await _unitOfWork.CourierRepo
                .GetByExpressionAsync(c => c.UserId == userId);

            if (courier == null)
                return NotFound("Courier not found");

            var packages = (await _unitOfWork.PackageRepo
       .GetAllWithIncludesAsync()) // جلب كل الباكجات مع الـ includes
       .Where(p => p.Status == PackageStatus.Pending)
       .Select(p => new
       {
           p.Id,
           pickupLat = p.Request.PickupLat,
           pickupLng = p.Request.PickupLng,
           p.ShipmentCost,
           CustomerName = p.Customer.User.UserName,
           CustomerEmail = p.Customer.User.Email,
           DestinationLat = p.Lat,
           DestinationLng = p.Lang,
           p.Status
       })
       .ToList();



            return Ok(packages);
        }


        //=============================== Earnings =============================

        [HttpGet("Earnings")]
        public async Task<IActionResult> GetCourierEarnings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courier = await _unitOfWork.CourierRepo.GetByExpressionAsync(c => c.UserId == userId);

            if (courier == null)
                return NotFound("Courier not found");

            // مثال لحساب الأرباح
            var deliveredPackages = (await _unitOfWork.PackageRepo.GetAllAsync())
                .Where(p => p.CourierID == courier.Id && p.Status == PackageStatus.Delivered);

            var totalEarnings = deliveredPackages.Sum(p => p.ShipmentCost);

            var earningsDto = new
            {
                totalEarnings,
                deliveredCount = deliveredPackages.Count(),
                lastDeliveryDate = deliveredPackages.OrderByDescending(p => p.DeliveredAt).FirstOrDefault()?.DeliveredAt
            };

            return Ok(earningsDto);
        }

        //--------------------------- CourierProfile -------------------
        [HttpGet("Profile")]
        public async Task<IActionResult> GetCourierProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var courier = await _unitOfWork.CourierRepo
                .GetByExpressionAsync(c => c.UserId == userId);

            if (courier == null)
                return NotFound("Courier not found");

            // جلب آخر موقع إذا موجود
            var lastLocation = courier.Locations?.OrderBy(l => l.RecordedAt).LastOrDefault();

            var dto = new CourierCompleteProfileDTO
            {
                Id = courier.Id.ToString(),
                Name = courier.User.UserName,
                Email = courier.User.Email,
                Phone = courier.User.PhoneNumber,
                VehicleType = courier.VehicleType.ToString(),
                LicenseNumber = courier.LicenseNumber,
                Rating = courier.Rating,
                CompletedDeliveries = courier.CompletedDeliveries,
                IsAvailable = courier.IsAvailable,
                IsOnline = courier.IsOnline,
                Locations = lastLocation != null
                    ? new CourierLocationDto
                    {
                        Lat = lastLocation.Lat,
                        Lng = lastLocation.Lng
                    }
                    : null
            };

            return Ok(dto);
        }



        //------------------------------------------

        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateCourierProfile([FromBody] CourierCompleteProfileDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var courier = await _unitOfWork.CourierRepo
                .GetByExpressionAsync(c => c.UserId == userId);

            if (courier == null)
                return NotFound("Courier not found");

            // تحديث الحقول المتاحة في DTO
            if (!string.IsNullOrEmpty(dto.Phone))
                courier.User.PhoneNumber = dto.Phone;

            if (!string.IsNullOrEmpty(dto.LicenseNumber))
                courier.LicenseNumber = dto.LicenseNumber;

            if (!string.IsNullOrEmpty(dto.VehicleType))
            {
                // تحويل الـ string لـ enum إذا لزم الأمر
                if (Enum.TryParse<VehicleType>(dto.VehicleType, out var vehicleType))
                    courier.VehicleType = vehicleType;
            }

            // يمكن تحديث الحالة المتاحة أو أونلاين إذا أحببت
            courier.IsAvailable = dto.IsAvailable;
            courier.IsOnline = dto.IsOnline;

            _unitOfWork.CourierRepo.Update(courier);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Profile updated successfully" });
        }





        //========================== otp-status ===========================

        [HttpGet("otp-status/{packageId}")]
public async Task<IActionResult> GetOTPStatus(int packageId)
{
    var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
    if (package == null)
        return NotFound("Package not found");

    return Ok(new
    {
        otpVerified = package.OTPVerified,
        status = package.Status
    });
}


    }
}