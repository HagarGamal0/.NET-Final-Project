using PickGo_backend.Models;

namespace PickGo_backend.DTOs.Package
{
    public class PackageUpdateDTO
    {
        public string? Description { get; set; }
        public float? Weight { get; set; }
        public bool? Fragile { get; set; }
        public DateTime? ExpireDate { get; set; }
        public float? ShipmentCost { get; set; }
        public string? Destination { get; set; }
        public double? Lat { get; set; }
        public double? Lang { get; set; }

        public PackageStatus? Status { get; set; }
        public string? ShipmentNotes { get; set; }
    }
}
