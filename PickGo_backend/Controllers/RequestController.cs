using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickGo_backend.DTOs.Request;
using PickGo_backend.Models;
using PickGo_backend.Models.Enums;
using PickGo_backend.Services; // Added for LynxTalismanService
using System.Security.Claims;

namespace PickGo_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Supplier")]
    public class RequestController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly LynxTalismanService _lynxService;

        public RequestController(UnitOfWork unitOfWork, IMapper mapper, LynxTalismanService lynxService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _lynxService = lynxService;
        }

[HttpPost]
public async Task<IActionResult> CreateRequest(RequestCreateDTO dto)
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var supplier = await _unitOfWork.SupplierRepo
        .GetByExpressionAsync(s => s.UserId == userId);

    if (supplier == null)
        return BadRequest("Supplier not found");

    var request = _mapper.Map<Request>(dto);
    request.SupplierId = supplier.Id;
    request.CreatedAt = DateTime.UtcNow;
    request.Status = RequestStatus.Pending;

    await _unitOfWork.RequestRepo.AddAsync(request);
    await _unitOfWork.SaveAsync();

    foreach (var p in dto.Packages)
    {
        var package = _mapper.Map<Package>(p);
        package.RequestID = request.Id;
        package.Status = PackageStatus.Pending;
        package.ReceiverPhone = p.ReceiverPhone;

        await _unitOfWork.PackageRepo.AddAsync(package);
    }

    await _unitOfWork.SaveAsync();

    var fullRequest = await _unitOfWork.RequestRepo.GetFullRequestAsync(request.Id);
    return Ok(_mapper.Map<RequestReadDTO>(fullRequest));
}




        [HttpGet]
public async Task<IActionResult> GetAll(
    string? search,
    string? status,
    string sortBy = "date",
    string sortDir = "desc"
)
{
    // ✅ FIX 1: Correctly resolve Supplier from UserId (GUID-safe)
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var supplier = await _unitOfWork.SupplierRepo
        .GetByExpressionAsync(s => s.UserId == userId);

    if (supplier == null)
        return Unauthorized("Supplier not found");

    var requests = await _unitOfWork.RequestRepo.GetBySupplierAsync(supplier.Id);

    // ✅ FIX 2: Multi-status filtering (?status=pending,assigned)
    if (!string.IsNullOrEmpty(status))
    {
        var statuses = status
    .Split(',', StringSplitOptions.RemoveEmptyEntries)
    .Select(s =>
    {
        if (Enum.TryParse<RequestStatus>(s, true, out var parsed))
            return (RequestStatus?)parsed;

        return null;
    })
    .Where(s => s.HasValue)
    .Select(s => s!.Value)
    .ToList();


        requests = requests
            .Where(r => statuses.Contains(r.Status))
            .ToList();
    }

    // Existing search logic
    if (!string.IsNullOrEmpty(search))
    {
        requests = requests.Where(r =>
            r.Source.Contains(search) ||
            r.Packages.Any(p => p.Description.Contains(search))
        ).ToList();
    }

    // Existing sorting logic
    requests = sortBy switch
    {
        "status" => sortDir == "asc"
            ? requests.OrderBy(r => r.Status).ToList()
            : requests.OrderByDescending(r => r.Status).ToList(),

        _ => sortDir == "asc"
            ? requests.OrderBy(r => r.CreatedAt).ToList()
            : requests.OrderByDescending(r => r.CreatedAt).ToList()
    };

    return Ok(_mapper.Map<List<RequestReadDTO>>(requests));
}


        [HttpGet("{id}")]
public async Task<IActionResult> GetOne(int id)
{
    var request = await _unitOfWork.RequestRepo.GetFullRequestAsync(id);
    if (request == null) return NotFound();

    return Ok(new
    {
        request.Id,
        request.Source,
        request.CreatedAt,
        request.Status,
        request.IsUrgent,
        request.ReadyForPickup,

        Supplier = new
        {
            request.SupplierId
        },

        Packages = request.Packages.Select(p => new
        {
            p.Id,
            p.Description,
            p.Weight,
            p.Status,
            p.ShipmentCost,
            p.Destination,
            p.ReceiverPhone,

            Courier = p.Courier == null ? null : new
            {
                p.Courier.Id,
                p.Courier.VehicleType,
                p.Courier.Rating
            }
        })
    });
}


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RequestUpdateDTO dto)
        {
            var request = await _unitOfWork.RequestRepo.GetAsync(id);
            if (request == null) return NotFound();

            _mapper.Map(dto, request);
            await _unitOfWork.SaveAsync();
            return Ok(_mapper.Map<RequestReadDTO>(request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _unitOfWork.RequestRepo.GetByIdAsync(id);
            if (request == null) return NotFound();

            _unitOfWork.RequestRepo.Delete(request);
            await _unitOfWork.SaveAsync();
            return Ok(new { message = "Request deleted" });
        }

        [HttpPut("{requestId}/assign/{courierId}")]
        public async Task<IActionResult> AssignRider(int requestId, int courierId)
        {
            var request = await _unitOfWork.RequestRepo.GetFullRequestAsync(requestId);
            if (request == null) return NotFound("Request not found");
            if (request.Status != RequestStatus.Pending)
                return BadRequest("Only pending requests can be assigned.");

            var courier = await _unitOfWork.CourierRepo.GetAsync(courierId);
            if (courier == null || !courier.IsAvailable)
                return BadRequest("Courier unavailable");

            foreach (var pkg in request.Packages)
                pkg.CourierID = courierId;

            request.Status = RequestStatus.Assigned;
            await _unitOfWork.SaveAsync();

            // Lynx Talisman Observation (SUPPLIER)
            await _lynxService.ExplainAssignmentAsync(request.Id, courierId, "SUPPLIER");

            return Ok(new { message = "Rider assigned successfully" });
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371;
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private async Task<Courier?> FindNearestAvailableCourier(Request request)
        {
            var couriers = (await _unitOfWork.CourierRepo.GetAllAsync())
                           .Where(c => c.Status == CourierStatus.Approved)
                           .ToList();

            Courier? nearest = null;
            double minDistance = double.MaxValue;

            foreach (var courier in couriers)
            {
                var lastLocation = courier.Locations?.OrderByDescending(l => l.RecordedAt).FirstOrDefault();
                if (lastLocation == null) continue;

                double distance = CalculateDistance(request.PickupLat, request.PickupLng, lastLocation.Lat, lastLocation.Lng);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = courier;
                }
            }

            return nearest;
        }
    }
}
