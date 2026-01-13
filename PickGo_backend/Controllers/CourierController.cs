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

            var packages = await _unitOfWork.PackageRepo.GetAllWithIncludesAsync();

            var result = packages
                .Where(p => p.CourierID == courier.Id &&
                           (p.Status == PackageStatus.Assigned ||
                            p.Status == PackageStatus.OutForDelivery))
                .Select(p => new
                {
                    p.Id,
                    RequestId = p.RequestID,
                    // Package data
                    p.Description,
                    p.Weight,
                    p.ShipmentCost,
                    Destination = p.Destination ?? "",
                    ReceiverPhone = p.ReceiverPhone ?? "",
                    p.Fragile,
                    Notes = p.ShipmentNotes ?? "",
                    // Request data (pickup info)
                    Source = p.Request?.Source ?? "",
                    PickupLat = p.Request?.PickupLat ?? 0,
                    PickupLng = p.Request?.PickupLng ?? 0,
                    IsUrgent = p.Request?.IsUrgent ?? false,
                    Priority = (p.Request?.IsUrgent ?? false) ? "urgent" : "normal",
                    // Package destination coords
                    DestinationLat = p.Lat ?? 0,
                    DestinationLng = p.Lang ?? 0,
                    Status = p.Status.ToString(),
                    // OTP info
                    p.DeliveryOTP,
                    p.OTPVerified,
                    // Customer info
                    CustomerName = p.Customer?.User?.UserName ?? "عميل",
                    CustomerPhone = p.ReceiverPhone ?? ""
                })
                .ToList();

            return Ok(result);
        }

        //====================================== AcceptPackage ===========================
        [HttpPost("AcceptPackage/{packageId}")]
public async Task<IActionResult> AcceptPackage(int packageId)
{
    var courier = await GetCourierFromToken();
    var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);

    if (package == null)
        return NotFound("Package not found");

    if (package.Status != PackageStatus.Pending)
        return BadRequest("Package is no longer available");

    // 🔥 ASSIGN HERE
    package.CourierID = courier.Id;
    package.Status = PackageStatus.Assigned;

    _unitOfWork.PackageRepo.Update(package);
    await _unitOfWork.SaveAsync();

    return Ok(new
    {
        message = "Package accepted",
        packageId = package.Id,
        courierId = courier.Id
    });
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
public async Task<IActionResult> DeliverPackage(
    int packageId,
    [FromBody] DeliverPackageDto dto)
{
    // 1️⃣ Get courier from token
    var courier = await GetCourierFromToken();

    // 2️⃣ Get package
    var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
    if (package == null)
        return NotFound("Package not found");

    // 3️⃣ Ownership check
    if (package.CourierID != courier.Id)
        return Forbid();

    // 4️⃣ Correct status check
    if (package.Status != PackageStatus.OutForDelivery)
        return BadRequest("Package not out for delivery");

    // 5️⃣ 🔒 OTP MUST BE VERIFIED
    if (!package.OTPVerified)
        return BadRequest("OTP must be verified before completing delivery");

    // 6️⃣ Create or update delivery proof
    var existingProof = (await _unitOfWork.DeliveryProofRepo.GetAllAsync())
        .FirstOrDefault(p => p.PackageID == package.Id);

    if (existingProof == null)
    {
        var proof = new DeliveryProof
        {
            PackageID = package.Id,
            CourierID = courier.Id,
            CustomerID = package.CustomerID,
            DeliveredAt = DateTime.UtcNow,
            Notes = dto.Notes
        };

        await _unitOfWork.DeliveryProofRepo.AddAsync(proof);
    }
    else
    {
        existingProof.DeliveredAt = DateTime.UtcNow;
        existingProof.Notes = dto.Notes;

        _unitOfWork.DeliveryProofRepo.Update(existingProof);
    }

    // 7️⃣ ✅ COMPLETE DELIVERY
    package.Status = PackageStatus.Delivered;
    package.DeliveredAt = DateTime.UtcNow;

    _unitOfWork.PackageRepo.Update(package);
    await _unitOfWork.SaveAsync();

    // 8️⃣ Done
    return Ok(new { message = "Package delivered successfully" });
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

    // 1. Validation Checks
    if (package == null) return NotFound("Package not found");
    if (package.CourierID != courier.Id) return Forbid();
    if (package.Status != PackageStatus.Assigned)
        return BadRequest("Package not ready for delivery");

    // 2. State Change
    package.Status = PackageStatus.OutForDelivery;

    // 3. OTP Generation Logic
    if (string.IsNullOrEmpty(package.DeliveryOTP))
    {
        // Use a cryptographically secure random number generator for production if possible
        package.DeliveryOTP = new Random().Next(100000, 999999).ToString();
        package.OTPVerified = false;

        // TODO: Send SMS via external service (e.g., Twilio, SendGrid)
        Console.WriteLine($"OTP for Package {package.Id}: {package.DeliveryOTP}");
    }

    // 4. Persistence
    _unitOfWork.PackageRepo.Update(package);
    await _unitOfWork.SaveAsync();

    return Ok(new { 
        message = "Delivery started. OTP has been sent to the customer.",
        status = package.Status, otp = package.DeliveryOTP.ToString()
    });
}



        //=====================================================================

        [HttpPost("VerifyOTP/{packageId}")]
public async Task<IActionResult> VerifyOTP(int packageId, [FromBody] string otp)
{
    var courier = await GetCourierFromToken();
    var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);

    if (package == null)
        return NotFound("Package not found");

    if (package.CourierID != courier.Id)
        return Forbid();

    if (package.Status != PackageStatus.OutForDelivery)
        return BadRequest("Package not out for delivery");

    if (package.DeliveryOTP != otp)
        return BadRequest("Invalid OTP");

    package.OTPVerified = true;

    _unitOfWork.PackageRepo.Update(package);
    await _unitOfWork.SaveAsync();

    return Ok(new { message = "OTP verified successfully" });
}




       [HttpGet("AvailableJobs")]
public async Task<IActionResult> GetAvailableJobs()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    var courier = await _unitOfWork.CourierRepo
        .GetByExpressionAsync(c => c.UserId == userId);

    if (courier == null)
        return NotFound("Courier not found");

    var packages = await _unitOfWork.PackageRepo.GetAllWithIncludesAsync();

    var result = packages
        .Where(p => p.Status == PackageStatus.Pending)
        .Select(p => new
        {
            p.Id,
            RequestId = p.Request != null ? p.Request.Id : 0,
            // Package data
            p.Description,
            p.Weight,
            p.ShipmentCost,
            Destination = p.Destination ?? "",
            ReceiverPhone = p.ReceiverPhone ?? "",
            p.Fragile,
            Notes = p.ShipmentNotes ?? "",
            // Request data (pickup info)
            Source = p.Request?.Source ?? "",
            PickupLat = p.Request?.PickupLat ?? 0,
            PickupLng = p.Request?.PickupLng ?? 0,
            IsUrgent = p.Request?.IsUrgent ?? false,
            Priority = (p.Request?.IsUrgent ?? false) ? "urgent" : "normal",
            // Package destination coords
            DestinationLat = p.Lat ?? 0,
            DestinationLng = p.Lang ?? 0,
            Status = p.Status.ToString(),
            // Customer info if available
            CustomerName = p.Customer?.User?.UserName ?? "عميل",
            CustomerPhone = p.ReceiverPhone ?? ""
        })
        .ToList();

    return Ok(result);
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
                .GetByUserIdAsync(userId);

            if (courier == null)
                return NotFound("Courier not found");

            // جلب آخر موقع إذا موجود
            var lastLocation = courier.Locations?.OrderBy(l => l.RecordedAt).LastOrDefault();

            var dto = new CourierCompleteProfileDTO
            {
                Id = courier.Id.ToString(),
                Name = courier.User?.UserName ?? "Unknown",
                Email = courier.User?.Email ?? "No Email",
                Phone = courier.User?.PhoneNumber ?? "No Phone",
                VehicleType = courier.VehicleType.ToString(),
                LicenseNumber = courier.LicenseNumber ?? "",
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


        // ================= Dashboard Summary =================
[HttpGet("DashboardSummary")]
public async Task<IActionResult> GetDashboardSummary()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var courier = await _unitOfWork.CourierRepo
        .GetByExpressionAsync(c => c.UserId == userId);

    if (courier == null)
        return NotFound("Courier not found");

    var packages = await _unitOfWork.PackageRepo.GetAllAsync();

    var myPackages = packages.Where(p => p.CourierID == courier.Id);

    return Ok(new
    {
        activeJobs = myPackages.Count(p =>
            p.Status == PackageStatus.Assigned ||
            p.Status == PackageStatus.OutForDelivery),

        completedJobs = myPackages.Count(p =>
            p.Status == PackageStatus.Delivered),

        totalEarnings = myPackages
            .Where(p => p.Status == PackageStatus.Delivered)
            .Sum(p => p.ShipmentCost)
    });
}

    // ================= Availability =================
[HttpGet("availability")]
public async Task<IActionResult> GetAvailability()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var courier = await _unitOfWork.CourierRepo
        .GetByExpressionAsync(c => c.UserId == userId);

    if (courier == null)
        return NotFound("Courier not found");

    return Ok(new
    {
        isAvailable = courier.IsAvailable
    });
}


// ================= End Shift =================
[HttpPost("endshift")]
public async Task<IActionResult> EndShift()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId))
        return Unauthorized();

    var courier = await _unitOfWork.CourierRepo
        .GetByExpressionAsync(c => c.UserId == userId);

    if (courier == null)
        return NotFound("Courier not found");

    // End shift logic
    courier.IsAvailable = false;
    courier.IsOnline = false;

    _unitOfWork.CourierRepo.Update(courier);
    await _unitOfWork.SaveAsync();

    // Optional: return shift summary (frontend already expects arrays safely)
    var packages = await _unitOfWork.PackageRepo.GetAllAsync();

    var todayOrders = packages
        .Where(p => p.CourierID == courier.Id &&
                    p.DeliveredAt != null &&
                    p.DeliveredAt.Value.Date == DateTime.UtcNow.Date)
        .ToList();

    return Ok(new
    {
        orders = todayOrders.Select(p => new
        {
            p.Id,
            p.ShipmentCost,
            p.DeliveredAt
        }),
        previousDays = Array.Empty<object>()
    });
}

// ================= Availability =================
[HttpPost("availability/toggle")]
public async Task<IActionResult> ToggleAvailability()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId))
        return Unauthorized();

    var courier = await _unitOfWork.CourierRepo
        .GetByExpressionAsync(c => c.UserId == userId);

    if (courier == null)
        return NotFound("Courier not found");

    courier.IsAvailable = !courier.IsAvailable;

    _unitOfWork.CourierRepo.Update(courier);
    await _unitOfWork.SaveAsync();

    return Ok(new
    {
        isAvailable = courier.IsAvailable
    });
}

    // ================= Active Jobs =================
[HttpGet("activeJobs")]
public async Task<IActionResult> GetActiveJobs()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var courier = await _unitOfWork.CourierRepo
        .GetByExpressionAsync(c => c.UserId == userId);

    if (courier == null)
        return NotFound("Courier not found");

    var packages = await _unitOfWork.PackageRepo.GetAllWithIncludesAsync();

    var result = packages
        .Where(p =>
            p.CourierID == courier.Id &&
            (p.Status == PackageStatus.Assigned ||
             p.Status == PackageStatus.OutForDelivery))
        .Select(p => new
        {
            p.Id,
            RequestId = p.RequestID,
            // Package data
            p.Description,
            p.Weight,
            p.ShipmentCost,
            Destination = p.Destination ?? "",
            ReceiverPhone = p.ReceiverPhone ?? "",
            // Request data (pickup info)
            Source = p.Request?.Source ?? "",
            PickupLat = p.Request?.PickupLat ?? 0,
            PickupLng = p.Request?.PickupLng ?? 0,
            IsUrgent = p.Request?.IsUrgent ?? false,
            // Package destination coords
            DestinationLat = p.Lat ?? 0,
            DestinationLng = p.Lang ?? 0,
            Status = p.Status.ToString(),
            // OTP info
            p.DeliveryOTP,
            p.OTPVerified,
            // Customer info
            CustomerName = p.Customer?.User?.UserName ?? "عميل"
        })
        .ToList();

    return Ok(result);
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
        status = package.Status, otp = package.DeliveryOTP
    });
}


// ===================== MyAssignedRequests (SAFE ADD) =====================
[HttpGet("MyAssignedRequests")]
[Authorize(Roles = "Courier")]
public async Task<IActionResult> MyAssignedRequests()
{
    try
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var courier = await _unitOfWork.CourierRepo
            .GetByExpressionAsync(c => c.UserId == userId);

        if (courier == null)
            return NotFound("Courier not found");

        // Fetch requests via packages
        var requests = (await _unitOfWork.RequestRepo.GetAllWithPackagesAsync())
            .Where(r =>
                (r.Packages?.Any(p =>
                    p.CourierID == courier.Id &&
                    (p.Status == PackageStatus.Assigned ||
                     p.Status == PackageStatus.OutForDelivery)
                ) ?? false)
            )
            .Select(r => new
            {
                requestId = r.Id,                // 🔒 CANONICAL ID
                createdAt = r.CreatedAt,
                isUrgent = r.IsUrgent,
                pickupLat = r.PickupLat,
                pickupLng = r.PickupLng,
                packagesCount = r.Packages?.Count ?? 0,
                codAmount = r.Packages?.Sum(p => p.ShipmentCost) ?? 0,
                status = r.Status.ToString()
            })
            .OrderByDescending(r => r.createdAt)
            .ToList();

        return Ok(requests);
    }
    catch (Exception ex)
    {
        return StatusCode(500, "MyAssignedRequests Error: " + ex.Message);
    }
}

    }
}