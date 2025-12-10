using PickGo_backend.Models;

namespace PickGo_backend.DTOs.Package
{
    public class PackageReadDTO
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public float Weight { get; set; }
        public bool Fragile { get; set; }
        public float ShipmentCost { get; set; }
        public string? Destination { get; set; }
        public double? Lat { get; set; }
        public double? Lang { get; set; }

        public PackageStatus Status { get; set; }
        public int RequestID { get; set; }
        public int CustomerID { get; set; }
    }
}
