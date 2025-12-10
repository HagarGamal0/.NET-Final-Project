using PickGo_backend.Models;
using PickGo_backend.DTOs.Package;

namespace PickGo_backend.DTOs.Request
{
    public class RequestReadDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Source { get; set; } = null!;
        public double PickupLat { get; set; }
        public double PickupLng { get; set; }
        public int SupplierId { get; set; }
        public RequestStatus Status { get; set; }

        public List<PackageReadDTO>? Packages { get; set; }
    }
}
