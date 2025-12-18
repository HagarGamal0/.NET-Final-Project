using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickGo_backend.DTOs.Supplier;
using PickGo_backend.Models;
using PickGo_backend.Models.Enums;
using PickGo_backend.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

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

        public SupplierController(UnitOfWork unitOfWork, IMapper mapper, OrderNotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        // -------------------- UC-SUP-02: Create New Parcel Request --------------------
        [HttpPost("CreateRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDTO dto)
        {
            var supplierId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var supplier = await _unitOfWork.SupplierRepo.GetByExpressionAsync(s => s.Id == supplierId);
            if (supplier == null) return NotFound("Supplier not found.");

            var request = new Request
            {
                SupplierId = supplier.Id,
                Source = dto.Source,
                PickupLat = dto.PickupLat,
                PickupLng = dto.PickupLng,
                Status = RequestStatus.Pending,
                IsUrgent = dto.Priority.ToLower() == "urgent",
                CreatedAt = DateTime.UtcNow
            };

            request.Packages = dto.Packages.Select(p => new Package
            {
                Description = p.Description,
                Weight = p.Weight,
                Fragile = p.Fragile,
                ExpireDate = p.ExpireDate,
                ShipmentCost = p.ShipmentCost,
                ShipmentNotes = p.Notes,
                CustomerID = p.CustomerID,
                Status = PackageStatus.Pending,
                Destination = p.Destination,
                Lat = p.Lat,
                Lang = p.Lng
            }).ToList();

            await _unitOfWork.RequestRepo.AddAsync(request);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Request created successfully", requestId = request.Id });
        }

        // -------------------- UC-SUP-06: Confirm Pickup Ready --------------------
        [HttpPost("ConfirmReady/{requestId}")]
        public async Task<IActionResult> ConfirmReady(int requestId)
        {
            var supplierId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var request = await _unitOfWork.RequestRepo.GetByIdAsync(requestId);
            if (request == null || request.SupplierId != supplierId)
                return NotFound("Request not found or unauthorized");

            request.ReadyForPickup = true;
            _unitOfWork.RequestRepo.Update(request);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Request marked as Ready for Pickup" });
        }

        // -------------------- UC-SUP-03: Assign Courier Manually --------------------
        [HttpPost("AssignCourier/{requestId}")]
        public async Task<IActionResult> AssignCourier(int requestId, [FromBody] AssignCourierDTO dto)
        {
            var supplierId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var request = await _unitOfWork.RequestRepo.GetByIdWithPackagesAsync(requestId);
            if (request == null || request.SupplierId != supplierId)
                return NotFound("Request not found or unauthorized");

            if (!request.ReadyForPickup)
                return BadRequest("Request is not ready for pickup");

            var courier = await _unitOfWork.CourierRepo.GetByExpressionAsync(c => c.Id == dto.CourierId);
            if (courier == null || !courier.IsOnline || courier.Status != CourierStatus.Approved)
                return BadRequest("Courier is not available");

            foreach (var package in request.Packages)
            {
                package.CourierID = courier.Id;
                package.Status = PackageStatus.Assigned;
            }

            request.Status = RequestStatus.Assigned;
            _unitOfWork.RequestRepo.Update(request);
            await _unitOfWork.SaveAsync();

            await _notificationService.NotifyCourierNearby(request.Id);

            return Ok(new { message = $"Courier {courier.Id} assigned successfully" });
        }

        // -------------------- UC-SUP-04: Track Order --------------------
        [HttpGet("TrackOrder/{requestId}")]
        public async Task<IActionResult> TrackOrder(int requestId)
        {
            var supplierId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var request = await _unitOfWork.RequestRepo.GetByIdWithPackagesAsync(requestId);
            if (request == null || request.SupplierId != supplierId)
                return NotFound("Request not found or unauthorized");

            var result = request.Packages.Select(p =>
            {
                var loc = p.Courier?.Locations?.OrderByDescending(l => l.RecordedAt).FirstOrDefault();
                return new
                {
                    packageId = p.Id,
                    status = p.Status.ToString(),
                    courierId = p.Courier?.Id,
                    courierName = p.Courier?.User?.UserName,
                    courierLocation = loc == null ? null : new { loc.Lat, loc.Lng, loc.RecordedAt }
                };
            });

            return Ok(new { requestId = request.Id, status = request.Status.ToString(), packages = result });
        }

        // -------------------- UC-SUP-05: View Delivered Orders & Reports --------------------
        [HttpGet("DeliveredOrders")]
        public async Task<IActionResult> DeliveredOrders(DateTime? fromDate, DateTime? toDate)
        {
            var supplierId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var orders = (await _unitOfWork.RequestRepo.GetAllWithPackagesAsync())
                .Where(r => r.SupplierId == supplierId &&
                            r.Status == RequestStatus.Delivered &&
                            (!fromDate.HasValue || r.CreatedAt >= fromDate) &&
                            (!toDate.HasValue || r.CreatedAt <= toDate))
                .ToList();

            var totalCOD = orders.Sum(r => r.Packages.Sum(p => p.ShipmentCost));

            return Ok(new
            {
                totalCOD,
                totalOrders = orders.Count,
                orders = orders.Select(r => new
                {
                    r.Id,
                    packages = r.Packages.Select(p => new { p.Id, CODAmount = p.ShipmentCost, p.Status })
                })
            });
        }
    }
}
