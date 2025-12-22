using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickGo_backend.DTOs.Courier;
using PickGo_backend.DTOs.DeliveryProof;
using PickGo_backend.Helpers;
using PickGo_backend.Models;
using PickGo_backend.Models.Enums;
using PickGo_backend.Services;
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
            var courier = await _unitOfWork.CourierRepo
                .GetByExpressionAsync(c => c.UserId == userId);

            if (courier == null)
                return NotFound("Courier not found");

            var packages = (await _unitOfWork.PackageRepo.GetAllAsync())
                .Where(p => p.CourierID == courier.Id &&
                       (p.Status == PackageStatus.Assigned ||
                        p.Status == PackageStatus.PickupInProgress ||
                        p.Status == PackageStatus.OutForDelivery))
                .Select(p => new
                {
                    p.Id,
                    Status = p.Status.ToString(),
                    CODAmount = p.ShipmentCost,  
                    Destination = p.Destination,
                    DropLat = p.Lat,
                    DropLng = p.Lang
                })
                .ToList();

            return Ok(packages);
        }


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


        [HttpPost("DeliverPackage/{packageId}")]
        public async Task<IActionResult> DeliverPackage(int packageId, [FromBody] DeliverPackageDto dto)
        {
            var courier = await GetCourierFromToken();
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);

            if (package == null)
                return NotFound("Package not found");

            if (package.CourierID != courier.Id)
                return Forbid("Not your package");

            if (package.Status != PackageStatus.OutForDelivery)
                return BadRequest("Package is not out for delivery");

            // تسجيل أي proof متوفر (توقيع، ملاحظات)
            var proof = new DeliveryProof
            {
                PackageID = package.Id,
                CourierID = courier.Id,
                CustomerID = package.CustomerID,
                DeliveredAt = DateTime.UtcNow
            };

            await _unitOfWork.DeliveryProofRepo.AddAsync(proof);

            // لا نغير حالة الطرد بعد، ننتظر VerifyOTP
            _unitOfWork.PackageRepo.Update(package);
            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "Delivery proof recorded. Waiting for customer OTP verification.",
                DeliveryOTP = package.DeliveryOTP // Optional: فقط للمراجعة أثناء التطوير
            });
        }

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


        [HttpPost("StartDelivery/{packageId}")]
        public async Task<IActionResult> StartDelivery(int packageId)
        {
            var courier = await GetCourierFromToken();

            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
            if (package == null)
                return NotFound("Package not found");

            if (package.CourierID != courier.Id)
                return Forbid("Not your package");

            if (package.Status != PackageStatus.Assigned)
                return BadRequest("Package not ready to start delivery");

            package.Status = PackageStatus.OutForDelivery;

            package.DeliveryOTP = GenerateOTP();
            package.OTPVerified = false;

            _unitOfWork.PackageRepo.Update(package);
            await _unitOfWork.SaveAsync();

            //  Send OTP to customer (later)
            await _emailService.SendEmailAsync(
     package.Customer.User.Email,
     "Your OTP for package delivery",
     $"Hello {package.Customer.User.UserName},<br>Your OTP for package delivery is: <b>{package.DeliveryOTP}</b>"
 );
            return Ok(new
            {
                message = "Delivery started",
                note = "OTP sent to customer"
            });
        }



[HttpPost("VerifyOTP/{packageId}")]
public async Task<IActionResult> VerifyOTP(int packageId, [FromBody] string otp)
{
    var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
    if (package == null)
        return NotFound("Package not found");

    // ❌ Wrong OTP
    if (package.DeliveryOTP != otp)
        return BadRequest("Invalid OTP");

    // ✅ Correct OTP → mark delivered
    package.OTPVerified = true;
    package.Status = PackageStatus.Delivered;
    package.DeliveredAt = DateTime.UtcNow;

    _unitOfWork.PackageRepo.Update(package);
    await _unitOfWork.SaveAsync();

    return Ok(new
    {
        message = "Package delivered successfully"
    });
}


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