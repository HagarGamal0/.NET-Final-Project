using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickGo_backend.DTOs.Courier;
using PickGo_backend.Helpers;
using PickGo_backend.Models;
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

        public CourierController(UnitOfWork unitOfWork, IGraphHopperService graphHopper)
        {
            _unitOfWork = unitOfWork;
            _graphHopper = graphHopper;
        }

        // -------------------- Get Online Couriers --------------------
        [HttpGet("Online")]
        public async Task<IActionResult> GetOnlineCouriers()
        {
            var couriers = (await _unitOfWork.CourierRepo.GetAllAsync())
                .Where(c => c.IsOnline && c.Status == "Approved")
                .Select(c => new
                {
                    c.Id,
                    c.UserId,
                    c.VehicleType,
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
            if (courier.Status != "Approved") return BadRequest("Courier is not approved yet.");

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
        [HttpPost("MatchCourier")]
        public async Task<IActionResult> MatchCourier([FromBody] CourierMatchRequest request)
        {
            var couriers = await _unitOfWork.CourierRepo.GetAllWithLocationsAsync();

            var nearby = couriers
       .Where(c => c.IsOnline && c.Status == "Approved" && c.Locations.Any())
       .Select(c =>
       {
           var loc = c.Locations.OrderByDescending(l => l.RecordedAt).First();
           var distance = GeoHelper.DistanceKm(
               request.PickupLat,
               request.PickupLng,
               loc.Lat,
               loc.Lng
           );

           return new { Courier = c, Location = loc, Distance = distance };
       })
       .Where(x => x.Distance <= 10) // 10 km radius
       .OrderBy(x => x.Distance)
       .Take(5)
       .ToList();

            var results = new List<object>();

            foreach (var c in nearby)
            {
                var vehicle = c.Courier.VehicleType.ToLower() switch
                {
                    "car" => "car",
                    "bike" => "bike",
                    "foot" => "foot",
                    _ => "car"
                };

                try
                {
                    var (km, eta) = await _graphHopper.GetRouteAsync(
                        c.Location.Lat, c.Location.Lng,
                        request.PickupLat, request.PickupLng,
                        vehicle
                    );

                    results.Add(new
                    {
                        c.Courier.Id,
                        c.Courier.VehicleType,
                        DistanceKm = Math.Round(km, 2),
                        EtaMinutes = Math.Round(eta, 1)
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"GraphHopper error for courier {c.Courier.Id}: {ex.Message}");
                }
            }

            if (!results.Any())
                return NotFound("No available route found for nearby couriers.");

            return Ok(results.OrderBy(r => ((dynamic)r).EtaMinutes));
        }
    }
}