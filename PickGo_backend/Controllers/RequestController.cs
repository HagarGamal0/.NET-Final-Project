using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickGo_backend.DTOs.Request;
using PickGo_backend.Models;
using PickGo_backend.Models.Enums;
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

        public RequestController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(RequestCreateDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var supplier = await _unitOfWork.SupplierRepo.GetByExpressionAsync(s => s.UserId == userId);
            if (supplier == null) return BadRequest("Supplier not found.");

            var request = _mapper.Map<Request>(dto);
            request.SupplierId = supplier.Id;
            request.CreatedAt = DateTime.UtcNow;
            request.Status = RequestStatus.Pending;

            // Save request first to get Id
            await _unitOfWork.RequestRepo.AddAsync(request);
            await _unitOfWork.SaveAsync();

            // Map and save packages using foreach
            var packages = dto.Packages.Select(p =>
            {
                var package = _mapper.Map<Package>(p);
                package.RequestID = request.Id;
                package.Status = PackageStatus.Pending;
                return package;
            }).ToList();

            foreach (var package in packages)
            {
                await _unitOfWork.PackageRepo.AddAsync(package);
            }
            await _unitOfWork.SaveAsync();

            // Auto-assign courier if non-urgent
            if (!dto.IsUrgent)
            {
                var courier = await FindNearestAvailableCourier(request);
                if (courier != null)
                {
                    foreach (var pkg in packages) pkg.CourierID = courier.Id;
                    courier.IsAvailable = false;
                    request.Status = RequestStatus.Accepted;
                    await _unitOfWork.SaveAsync();
                }
            }

            return Ok(_mapper.Map<RequestReadDTO>(request));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(string? search, string sortBy = "date", string sortDir = "desc")
        {
            var supplierId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var requests = await _unitOfWork.RequestRepo.GetBySupplierAsync(supplierId);

            if (!string.IsNullOrEmpty(search))
                requests = requests.Where(r =>
                    r.Source.Contains(search) ||
                    r.Packages.Any(p => p.Description.Contains(search))
                ).ToList();

            requests = sortBy switch
            {
                "status" => sortDir == "asc" ? requests.OrderBy(r => r.Status).ToList() : requests.OrderByDescending(r => r.Status).ToList(),
                _ => sortDir == "asc" ? requests.OrderBy(r => r.CreatedAt).ToList() : requests.OrderByDescending(r => r.CreatedAt).ToList()
            };

            return Ok(_mapper.Map<List<RequestReadDTO>>(requests));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var request = await _unitOfWork.RequestRepo.GetFullRequestAsync(id);
            if (request == null) return NotFound();

            return Ok(_mapper.Map<RequestReadDTO>(request));
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
            var request = await _unitOfWork.RequestRepo.GetAsync(requestId);
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
