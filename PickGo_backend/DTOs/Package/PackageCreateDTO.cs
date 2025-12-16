namespace PickGo_backend.DTOs.Package
{
    public class PackageCreateDTO
    {
        public string Description { get; set; } = null!;
        public float Weight { get; set; }
        public bool Fragile { get; set; }
        public DateTime ExpireDate { get; set; }
        public float ShipmentCost { get; set; }
        public string? Destination { get; set; }
        public double? Lat { get; set; }
        public double? Lang { get; set; }
        public int CustomerID { get; set; }
    }
}
