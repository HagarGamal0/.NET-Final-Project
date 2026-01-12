using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickGo_backend.DTOs.Courier;
using PickGo_backend.DTOs.Supplier;
using PickGo_backend.Models;
using PickGo_backend.Models.Enums;
using PickGo_backend.Services;
using PickGo_backend.DTOs.Request;
using System.Security.Claims;

namespace PickGo_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Supplier")]
    public class SupplierController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly OrderNotificationService _notificationService;
        private readonly CourierMatchingService _matchingService;
        private readonly LynxTalismanService _lynxService;

        public SupplierController(
            UnitOfWork unitOfWork,
            IMapper mapper,
            OrderNotificationService notificationService,
            CourierMatchingService matchingService,
            LynxTalismanService lynxService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _matchingService = matchingService;
            _lynxService = lynxService;
        }

        // -------------------- UC-SUP-02: Create New Parcel Request --------------------
 [HttpPost("CreateRequest")]
public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDTO dto)
{
    try
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var supplier = await _unitOfWork.SupplierRepo
            .GetSupplierWithIncludesAsync(userId!);

        if (supplier == null)
            return Unauthorized("Supplier not found");

        if (dto.Packages == null || !dto.Packages.Any())
            return BadRequest("At least one package is required");

        var request = new Request
        {
            SupplierId = supplier.Id,
            Source = dto.Source,
            PickupLat = dto.PickupLat,
            PickupLng = dto.PickupLng,
            Status = RequestStatus.Pending,
            IsUrgent = dto.Priority?.ToLower() == "urgent",
            CreatedAt = DateTime.UtcNow,

            Packages = dto.Packages.Select(p =>
            {
                // ✅ REQUIRED: Guest customer must have phone number
                if (string.IsNullOrWhiteSpace(p.ReceiverPhone))
                    throw new Exception("Receiver phone number is required for guest delivery");

                return new Package
                {
                    Description = p.Description,
                    Weight = p.Weight,
                    Fragile = p.Fragile,
                    ExpireDate = p.ExpireDate,
                    ShipmentCost = p.ShipmentCost,
                    ShipmentNotes = p.Notes,

                    // ✅ Guest-only customer (no account, no ID)
                    CustomerID = null,

                    // ✅ SINGLE SOURCE OF TRUTH FOR CUSTOMER
                    ReceiverPhone = p.ReceiverPhone,

                    Status = PackageStatus.Pending,
                    Destination = p.Destination,
                    Lat = p.Lat,
                    Lang = p.Lng
                };
            }).ToList()
        };

        await _unitOfWork.RequestRepo.AddAsync(request);
        await _unitOfWork.SaveAsync();
        var requestNumber = request.Id;

        await _notificationService.NotifyNewRequest(requestNumber);

        return Ok(new
        {
            message = "Request created successfully",
            requestNumber = request.Id
        });
    }
    catch (Exception ex)
    {
        // ✅ Demo-safe, readable backend error
        return StatusCode(500, ex.Message);
    }
}


        // -------------------- UC-SUP-06: Confirm Pickup Ready --------------------
[HttpPost("ConfirmReady/{requestId}")]
public async Task<IActionResult> ConfirmReady(int requestId)
{
    try
    {
        var supplier = await GetSupplier();

        var request = await _unitOfWork.RequestRepo
            .GetByIdWithPackagesAsync(requestId); // ⬅️ ensure packages loaded

        if (request == null || request.SupplierId != supplier.Id)
            return NotFound("Request not found or unauthorized");

        // ✅ ORIGINAL LOGIC (UNCHANGED)
        request.ReadyForPickup = true;
        _unitOfWork.RequestRepo.Update(request);
        await _unitOfWork.SaveAsync();

        // 🐆 ADDITION: URGENT AUTO-ASSIGN (NON-BLOCKING)
        if (request.IsUrgent)
        {
            try
            {
                // 🔴 DEMO: single courier
                var demoCourierUserId = "1926645b-dc54-45c7-b576-175bfc9058f2";

                var courier = (await _unitOfWork.CourierRepo.GetAllWithUserAsync())
                    .FirstOrDefault(c =>
                        c.User != null &&
                        c.UserId == demoCourierUserId
                    );

                if (courier != null && request.Packages != null)
                {
                    foreach (var package in request.Packages)
                    {
                        package.CourierID = courier.Id;
                        package.Status = PackageStatus.Assigned;
                    }

                    request.Status = RequestStatus.Assigned;

                    _unitOfWork.RequestRepo.Update(request);
                    await _unitOfWork.SaveAsync();

                    // 🔔 Notify courier (SignalR)
                    await _notificationService.NotifyCourierNearby(request.Id);

                    // 🐆 Lynx explanation
                    await _lynxService.ExplainAssignmentAsync(
                        request.Id,
                        courier.Id,
                        "LYNX_AUTO_URGENT"
                    );
                }
            }
            catch
            {
                // ❗ Swallow errors to NEVER break original ConfirmReady flow
                // Auto-assign failure must NOT block supplier workflow
            }
        }

        // ✅ ORIGINAL RESPONSE (UNCHANGED)
        return Ok(new
        {
            message = request.IsUrgent
                ? "🚨 Urgent request marked ready (auto-assign attempted 🐆)"
                : "Request marked as Ready for Pickup"
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, ex.Message);
    }
}


        // -------------------- UC-SUP-07: Cancel Parcel --------------------
        [HttpPost("CancelParcel/{requestId}")]
        public async Task<IActionResult> CancelParcel(int requestId)
        {
            try
            {
                var supplier = await GetSupplier();

                var request = await _unitOfWork.RequestRepo.GetByIdWithPackagesAsync(requestId);
                if (request == null || request.SupplierId != supplier.Id)
                    return NotFound("Request not found or unauthorized");

                if (request.Status == RequestStatus.PickupInProgress ||
                    request.Status == RequestStatus.Delivered)
                    return BadRequest("Cannot cancel a request that is already in progress or delivered.");

                request.Status = RequestStatus.Cancelled;

                foreach (var pkg in request.Packages ?? new List<Package>())
                    pkg.Status = PackageStatus.Cancelled;

                _unitOfWork.RequestRepo.Update(request);
                await _unitOfWork.SaveAsync();

                return Ok(new { message = "Request and packages cancelled successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // -------------------- UC-SUP-03: Assign Courier --------------------
        [HttpPost("AssignCourier/{requestId}")]
        public async Task<IActionResult> AssignCourier(int requestId, [FromBody] AssignCourierDTO dto)
        {
            try
            {
                var supplier = await GetSupplier();

                var request = await _unitOfWork.RequestRepo.GetByIdWithPackagesAsync(requestId);
                if (request == null || request.SupplierId != supplier.Id)
                    return NotFound("Request not found or unauthorized");

                if (!request.ReadyForPickup)
                    return BadRequest("Request is not ready for pickup");

                var courier = await _unitOfWork.CourierRepo
                    .GetByExpressionAsync(c => c.Id == dto.CourierId);

                if (courier == null || !courier.IsOnline || courier.Status != CourierStatus.Approved)
                    return BadRequest("Courier is not available");

                foreach (var pkg in request.Packages)
                {
                    pkg.CourierID = courier.Id;
                    pkg.Status = PackageStatus.Assigned;
                }

                request.Status = RequestStatus.Assigned;
                _unitOfWork.RequestRepo.Update(request);
                await _unitOfWork.SaveAsync();

                await _notificationService.NotifyCourierNearby(request.Id);
                await _lynxService.ExplainAssignmentAsync(request.Id, courier.Id, "SUPPLIER");

                return Ok(new { message = $"Courier {courier.Id} assigned successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // -------------------- UC-SUP-04: Track Order --------------------
        [HttpGet("TrackOrder/{requestId}")]
        public async Task<IActionResult> TrackOrder(int requestId)
        {
            try
            {
                var supplier = await GetSupplier();

                var request = await _unitOfWork.RequestRepo.GetByIdWithPackagesAsync(requestId);
                if (request == null || request.SupplierId != supplier.Id)
                    return NotFound("Request not found or unauthorized");

                var result = (request.Packages ?? new List<Package>()).Select(p =>
                {
                    var loc = p.Courier?.Locations?
                        .OrderByDescending(l => l.RecordedAt)
                        .FirstOrDefault();

                    return new
                    {
                        packageId = p.Id,
                        status = p.Status.ToString(),
                        courierId = p.Courier?.Id,
                        courierName = p.Courier?.User?.UserName,
                        courierLocation = loc == null ? null : new
                        {
                            loc.Lat,
                            loc.Lng,
                            loc.RecordedAt
                        }
                    };
                });

                return Ok(new
                {
                    requestId = request.Id,
                    status = request.Status.ToString(),
                    packages = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // -------------------- UC-SUP-05: Delivered Orders --------------------
        [HttpGet("DeliveredOrders")]
        public async Task<IActionResult> DeliveredOrders(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var supplier = await GetSupplier();

                var orders = (await _unitOfWork.RequestRepo.GetAllWithPackagesAsync())
                    .Where(r =>
                        r.SupplierId == supplier.Id &&
                        r.Status == RequestStatus.Delivered &&
                        (!fromDate.HasValue || r.CreatedAt >= fromDate) &&
                        (!toDate.HasValue || r.CreatedAt <= toDate))
                    .ToList();

                var totalCOD = orders.Sum(r => r.Packages?.Sum(p => p.ShipmentCost) ?? 0);

                return Ok(new
                {
                    totalCOD,
                    totalOrders = orders.Count,
                    orders = orders.Select(r => new
                    {
                        r.Id,
                        packages = r.Packages?.Select(p => new
                        {
                            p.Id,
                            CODAmount = p.ShipmentCost,
                            p.Status
                        }) ?? Enumerable.Empty<object>()
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // -------------------- Match Courier --------------------
        [HttpPost("MatchCourier")]
        public async Task<IActionResult> MatchCourier([FromBody] CourierMatchRequest request)
        {
            var results = await _matchingService.GetRankedCouriersAsync(request.PickupLat, request.PickupLng);
            if (!results.Any())
                return NotFound("No available route found");

            return Ok(results.Select(r => new
            {
                r.Courier.Id,
                VehicleType = r.Courier.VehicleType.ToString(),
                r.DistanceKm,
                r.EtaMinutes
            }));
        }

        // -------------------- Dashboard --------------------
        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var supplier = await GetSupplier();
                var requests = supplier.Requests ?? new List<Request>();

                var today = DateTime.UtcNow.Date;

                return Ok(new
                {
                    pendingCount = requests.Count(r => r.Status == RequestStatus.Pending),
                    readyForPickupCount = requests.Count(r => r.Status == RequestStatus.Assigned),
                    inTransitCount = requests.Count(r => r.Status == RequestStatus.PickupInProgress),
                    deliveredTodayCount = requests.Count(r =>
                        r.Status == RequestStatus.Delivered && r.CreatedAt.Date == today),
                    totalCodToday = requests
                        .Where(r => r.Status == RequestStatus.Delivered && r.CreatedAt.Date == today)
                        .Sum(r => r.Packages?.Sum(p => p.ShipmentCost) ?? 0)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // -------------------- Profile (INLINE DTO) --------------------
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var supplier = await GetSupplier();

            return Ok(new SupplierProfileDTO
            {
                Id = supplier.Id,
                Name = supplier.User?.UserName ?? "Unknown",
                Email = supplier.User?.Email ?? "N/A",
                Phone = supplier.User?.PhoneNumber ?? "N/A",
                TotalRequests = supplier.Requests?.Count ?? 0
            });
        }

        // -------------------- Explanation --------------------
        [HttpGet("Explanation/{requestId}")]
        public async Task<IActionResult> GetExplanation(int requestId)
        {
            try
            {
                var supplier = await GetSupplier();

                var request = await _unitOfWork.RequestRepo.GetByIdAsync(requestId);
                if (request == null || request.SupplierId != supplier.Id)
                    return Unauthorized();

                var obs = await _unitOfWork.AssignmentObservationRepo
                    .GetLatestForRequest(requestId);

                if (obs == null)
                    return NotFound("No explanation found");

                return Ok(obs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // -------------------- HELPER --------------------
        private async Task<Supplier> GetSupplier()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var supplier = await _unitOfWork.SupplierRepo
                .GetSupplierWithIncludesAsync(userId!);

            if (supplier == null)
                throw new Exception("Supplier not found");

            return supplier;
        }

[HttpGet("AvailableCarriers/{requestId}")]
public async Task<IActionResult> GetAvailableCarriers(int requestId)
{
    try
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var supplier = await _unitOfWork.SupplierRepo
            .GetSupplierWithIncludesAsync(userId!);
        var demoCourierUserId = "1926645b-dc54-45c7-b576-175bfc9058f2";
        if (supplier == null)
            return Unauthorized("Supplier not found");

        var request = await _unitOfWork.RequestRepo.GetByIdAsync(requestId);
        if (request == null || request.SupplierId != supplier.Id)
            return NotFound("Request not found");

        var couriers = (await _unitOfWork.CourierRepo.GetAllWithUserAsync())
    .Where(c =>
        c.User != null &&                       // ✅ CRITICAL FIX
        c.UserId == demoCourierUserId
    )
    .Select(c => new
    {
        c.Id,
        Name = c.User!.UserName,
        c.VehicleType,
        c.Rating,
        c.IsOnline,
        c.IsAvailable
    })
    .ToList();

        return Ok(couriers);
    }
    catch (Exception ex)
    {
        return StatusCode(500, "AvailableCarriers Error: " + ex.Message);
    }
}

private async Task<bool> AutoAssignUrgentRequestAsync(Request request)
{
    // 🔴 DEMO: restrict to one courier
    var demoCourierUserId = "1926645b-dc54-45c7-b576-175bfc9058f2";

    var courier = (await _unitOfWork.CourierRepo.GetAllWithUserAsync())
        .FirstOrDefault(c =>
            c.User != null &&
            c.UserId == demoCourierUserId
        );

    if (courier == null)
        return false;

    foreach (var package in request.Packages!)
    {
        package.CourierID = courier.Id;
        package.Status = PackageStatus.Assigned;
    }

    request.Status = RequestStatus.Assigned;

    _unitOfWork.RequestRepo.Update(request);
    await _unitOfWork.SaveAsync();

    // 🔔 Notify courier (SignalR)
    await _notificationService.NotifyCourierNearby(request.Id);

    // 🐆 Lynx explanation
    await _lynxService.ExplainAssignmentAsync(
        request.Id,
        courier.Id,
        "LYNX_AUTO_URGENT"
    );

    return true;
}


    }
}
