namespace PickGo_backend.DTOs.Supplier
{
    public class CreatePackageDTO
    {
        public string Description { get; set; } = null!;
        public float Weight { get; set; }
        public bool Fragile { get; set; }
        public DateTime ExpireDate { get; set; }
        public float ShipmentCost { get; set; }

        // Drop-off info
        public string Destination { get; set; } = null!; // customer address
        public double Lat { get; set; }
        public double Lng { get; set; }

        public string? Notes { get; set; }

        public int CustomerID { get; set; } // Link to the customer
    }
}
