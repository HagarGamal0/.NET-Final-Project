using PickGo_backend.Helpers;
using PickGo_backend.Models;
using PickGo_backend.Models.Enums;
using PickGo_backend.Repositories;
using PickGo_backend; // For UnitOfWork
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickGo_backend.Services
{
    public class CourierMatchingResult
    {
        public Courier Courier { get; set; }
        public CourierLocation Location { get; set; }
        public double DistanceKm { get; set; }
        public double EtaMinutes { get; set; }
    }

    public class CourierMatchingService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IGraphHopperService _graphHopper;

        public CourierMatchingService(UnitOfWork unitOfWork, IGraphHopperService graphHopper)
        {
            _unitOfWork = unitOfWork;
            _graphHopper = graphHopper;
        }

        public async Task<List<CourierMatchingResult>> GetRankedCouriersAsync(double pickupLat, double pickupLng, int limit = 5)
        {
            var couriers = await _unitOfWork.CourierRepo.GetAllWithLocationsAsync();

            var nearby = couriers
                .Where(c => c.IsOnline && c.Status == CourierStatus.Approved && c.Locations.Any())
                .Select(c =>
                {
                    var loc = c.Locations.OrderByDescending(l => l.RecordedAt).First();
                    var distance = GeoHelper.DistanceKm(pickupLat, pickupLng, loc.Lat, loc.Lng);
                    return new { Courier = c, Location = loc, Distance = distance };
                })
                .Where(x => x.Distance <= 10)
                .OrderBy(x => x.Distance)
                .Take(limit) // strict filter before graphhopper to save calls
                .ToList();

            var results = new List<CourierMatchingResult>();

            foreach (var c in nearby)
            {
                var vehicle = c.Courier.VehicleType.ToString().ToLower();
                try
                {
                    var (km, eta) = await _graphHopper.GetRouteAsync(
                        c.Location.Lat, c.Location.Lng,
                        pickupLat, pickupLng,
                        vehicle
                    );

                    results.Add(new CourierMatchingResult
                    {
                        Courier = c.Courier,
                        Location = c.Location,
                        DistanceKm = Math.Round(km, 2),
                        EtaMinutes = Math.Round(eta, 1)
                    });
                }
                catch (Exception)
                {
                    // Fallback to straight line if graphhopper fails? 
                    // Controller code printed to console.
                    // We'll skip or use GeoHelper distance + rough speed.
                    // For now, skip to match controller behavior.
                }
            }

            return results.OrderBy(r => r.EtaMinutes).ToList();
        }
    }
}
