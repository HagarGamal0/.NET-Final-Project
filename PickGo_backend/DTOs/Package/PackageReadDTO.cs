using PickGo_backend.Models;

namespace PickGo_backend.DTOs.Package
{
    public class PackageReadDTO
    {
        public int Id { get; set; }

        // ✅ UI-safe tracking reference
        public string TrackingCode => $"PKG-{Id}";

        public string Description { get; set; } = "—";
        public float Weight { get; set; }
        public bool Fragile { get; set; }
        public float ShipmentCost { get; set; }

        // 🔴 FIX: destination must NEVER be null for UI
        public string Destination { get; set; } = "—";

        // 🔴 FIX: coordinates default to 0 instead of null
        public double Lat { get; set; }
        public double Lang { get; set; }

        public PackageStatus Status { get; set; }

        public int RequestID { get; set; }
        public int? CustomerID { get; set; }
    }
}
