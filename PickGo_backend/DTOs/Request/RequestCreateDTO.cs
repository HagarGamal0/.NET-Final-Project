using PickGo_backend.DTOs.Package;

namespace PickGo_backend.DTOs.Request


{
    public class RequestCreateDTO
    {
        public string Source { get; set; } = null!;
        public double PickupLat { get; set; }
        public double PickupLng { get; set; }
        public bool IsUrgent { get; set; }

        public List<PackageCreateDTO> Packages { get; set; } = new();
    }
}
