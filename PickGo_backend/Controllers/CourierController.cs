

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickGo_backend.Context;
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
        

        public CourierController(
            UnitOfWork unitOfWork,
            IGraphHopperService graphHopper,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _graphHopper = graphHopper;
            _emailService = emailService;
        }

        //====================== dashboard ==============

        [HttpGet("DashboardSummary")]
        public async Task<IActionResult> DashboardSummary()
        {
            var courier = await GetCurrentCourier();
            if (courier == null) return NotFound("Courier not found");

            // --- الأرباح ---
            var deliveredPackages = (await _unitOfWork.PackageRepo.GetAllAsync())
                .Where(p => p.CourierID == courier.Id && p.Status == PackageStatus.Delivered);
            var totalEarnings = deliveredPackages.Sum(p => p.ShipmentCost);

            // --- الطلبات الحالية ---
            var currentOrders = (await _unitOfWork.PackageRepo.GetAllAsync())
                .Where(p => p.CourierID == courier.Id &&
                           (p.Status == PackageStatus.Assigned || p.Status == PackageStatus.OutForDelivery))
                .Select(p => new
                {
                    p.Id,
                    Status = p.Status.ToString(),
                    CODAmount = p.ShipmentCost,
                    DestinationLat = p.Lat,
                    DestinationLng = p.Lang
                }).ToList();

            // --- الطلبات المتاحة ---
            var availableJobs = (await _unitOfWork.PackageRepo.GetAllWithIncludesAsync())
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
                }).ToList();

            // --- الحالة الحالية ---
            var availability = new { isAvailable = courier.IsAvailable, isOnline = courier.IsOnline };

            // --- آخر موقع ---
            var lastLocation = courier.Locations?.OrderBy(l => l.RecordedAt).LastOrDefault();
            var locationDto = lastLocation != null
                ? new { lastLocation.Lat, lastLocation.Lng }
                : null;

            // --- مخلص اليوم (End Shift) ---
            var today = DateTime.UtcNow.Date;
            var ordersToday = deliveredPackages
                .Where(p => p.DeliveredAt.HasValue && p.DeliveredAt.Value.Date == today)
                .Select(p => new
                {
                    p.Id,
                    time = p.DeliveredAt,
                    amount = p.ShipmentCost,
                    status = p.Status.ToString()
                }).ToList();

            var previousDays = deliveredPackages
                .Where(p => p.DeliveredAt.HasValue)
                .GroupBy(p => p.DeliveredAt!.Value.Date)
                .Select(g => new
                {
                    date = g.Key,
                    ordersCount = g.Count(),
                    totalAmount = g.Sum(x => x.ShipmentCost)
                })
                .OrderByDescending(x => x.date)
                .Take(7)
                .ToList();

            return Ok(new
            {
                totalEarnings,
                currentOrders,
                availableJobs,
                availability,
                lastLocation = locationDto,
                ordersToday,
                previousDays
            });
        }



        // =====================================================
        // Helper: Current Courier
        // =====================================================
        private async Task<Courier?> GetCurrentCourier()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return null;

            return await _unitOfWork.CourierRepo
                .GetByExpressionAsync(c => c.UserId == userId);
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
        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetCourierProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // جلب الكورير مع البيانات
            var courier = await _unitOfWork.CourierRepo.GetCourierWithProfileAsync(userId);

            if (courier == null)
                return NotFound("Courier not found");

            // عمل DTO للبروفايل
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
                PhotoUrl = courier.PhotoUrl,
                Address = courier.address,
                LicensePhotoFront = courier.LicensePhotoFront,
                LicensePhotoBack = courier.LicensePhotoBack,
                VehicleLicensePhotoFront = courier.VehcelLicensePhotoFront,
                VehicleLicensePhotoBack = courier.VehcelLicensePhotoBack,
                IdPhotoUrl = courier.IdPhotoUrl,
                Locations = courier.Locations?
                    .OrderByDescending(l => l.RecordedAt)
                    .Select(l => new CourierLocationDto
                    {
                        Lat = l.Lat,
                        Lng = l.Lng
                    }).ToList()
            };

            return Ok(dto);
        }



        //------------------------------------------update
        private async Task<string> SaveFile(IFormFile file)
        {
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var path = Path.Combine(uploads, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/{fileName}";
        }


        [Authorize]
        [HttpPut("updateProfile")]
        public async Task<IActionResult> UpdateCourierProfile(
    [FromForm] UpdateCourierProfileDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var courier = await _unitOfWork.CourierRepo.GetCourierWithProfileAsync(userId);
            if (courier == null)
                return NotFound("Courier not found");

            courier.User.PhoneNumber = dto.Phone ?? courier.User.PhoneNumber;
            courier.LicenseNumber = dto.LicenseNumber ?? courier.LicenseNumber;
            courier.address = dto.Address ?? courier.address;

            if (!string.IsNullOrEmpty(dto.VehicleType) &&
                Enum.TryParse(dto.VehicleType, out VehicleType vt))
                courier.VehicleType = vt;

            courier.IsAvailable = dto.IsAvailable;
            courier.IsOnline = dto.IsOnline;

            if (dto.Photo != null)
                courier.PhotoUrl = await SaveFile(dto.Photo);

            if (dto.LicensePhotoFront != null)
                courier.LicensePhotoFront = await SaveFile(dto.LicensePhotoFront);

            if (dto.LicensePhotoBack != null)
                courier.LicensePhotoBack = await SaveFile(dto.LicensePhotoBack);

            if (dto.VehicleLicensePhotoFront != null)
                courier.VehcelLicensePhotoFront = await SaveFile(dto.VehicleLicensePhotoFront);

            if (dto.VehicleLicensePhotoBack != null)
                courier.VehcelLicensePhotoBack = await SaveFile(dto.VehicleLicensePhotoBack);

            if (dto.IdPhoto != null)
                courier.IdPhotoUrl = await SaveFile(dto.IdPhoto);

            _unitOfWork.CourierRepo.Update(courier);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Profile updated successfully" });
        }


        // =====================================================
        // Toggle Availability (زر واحد)
        // =====================================================
        [HttpPost("availability/toggle")]
        public async Task<IActionResult> ToggleAvailability()
        {
            var courier = await GetCurrentCourier();
            if (courier == null)
                return NotFound("Courier not found");

            courier.IsAvailable = !courier.IsAvailable;
            courier.IsOnline = courier.IsAvailable;

            _unitOfWork.CourierRepo.Update(courier);
            await _unitOfWork.SaveAsync();

            // لو بقى غير متاح → End Shift
            if (!courier.IsAvailable)
            {
                return await EndShiftInternal();
            }

            return Ok(new
            {
                isAvailable = courier.IsAvailable,
                message = "Courier is now AVAILABLE"
            });
        }

        // =====================================================
        // Get Availability
        // =====================================================
        [HttpGet("availability")]
        public async Task<IActionResult> GetAvailability()
        {
            var courier = await GetCurrentCourier();
            if (courier == null)
                return NotFound("Courier not found");

            return Ok(new { isAvailable = courier.IsAvailable });
        }

        // =====================================================
        // End Shift (API)
        // =====================================================
        [HttpPost("endshift")]
        public async Task<IActionResult> EndShift()
        {
            return await EndShiftInternal();
        }

        // =====================================================
        // End Shift Logic (مش API)
        // =====================================================
        private async Task<IActionResult> EndShiftInternal()
        {
            var courier = await GetCurrentCourier();
            if (courier == null)
                return NotFound("Courier not found");

            var today = DateTime.UtcNow.Date;

            var orders = (await _unitOfWork.PackageRepo.GetAllAsync())
                .Where(p =>
                    p.CourierID == courier.Id &&
                    p.Status == PackageStatus.Delivered &&
                    p.DeliveredAt.HasValue &&
                    p.DeliveredAt.Value.Date == today)
                .Select(p => new
                {
                    p.Id,
                    time = p.DeliveredAt,
                    amount = p.ShipmentCost,
                    status = p.Status.ToString()
                })
                .ToList();

            var previousDays = (await _unitOfWork.PackageRepo.GetAllAsync())
                .Where(p =>
                    p.CourierID == courier.Id &&
                    p.Status == PackageStatus.Delivered &&
                    p.DeliveredAt.HasValue)
                .GroupBy(p => p.DeliveredAt!.Value.Date)
                .Select(g => new
                {
                    date = g.Key,
                    ordersCount = g.Count(),
                    totalAmount = g.Sum(x => x.ShipmentCost)
                })
                .OrderByDescending(x => x.date)
                .Take(7)
                .ToList();

            return Ok(new
            {
                isAvailable = false,
                orders,
                previousDays
            });
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



        // ===================== Active Jobs =====================
        [HttpGet("ActiveJobs")]
        public async Task<IActionResult> GetActiveJobs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var courier = await _unitOfWork.CourierRepo
                .GetByExpressionAsync(c => c.UserId == userId);

            if (courier == null)
                return NotFound("Courier not found");

            // جلب الطلبات الحالية (اللي أخدها الكورير)
            var activeJobs = (await _unitOfWork.PackageRepo.GetAllAsync())
                .Where(p => p.CourierID == courier.Id &&
                           (p.Status == PackageStatus.Assigned ||
                            p.Status == PackageStatus.OutForDelivery))
                .Select(p => new
                {
                    p.Id,
                    Status = p.Status.ToString(),
                    CODAmount = p.ShipmentCost,
                    DestinationLat = p.Lat,
                    DestinationLng = p.Lang,
                    CustomerName = p.Customer.User.UserName,
                    CustomerEmail = p.Customer.User.Email
                })
                .ToList();

            return Ok(activeJobs);
        }

    }
}
