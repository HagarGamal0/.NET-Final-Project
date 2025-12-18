namespace PickGo_backend.DTOs.Supplier
{
    public class CreateRequestDTO
    {
        // Pickup info (Source)
        public string Source { get; set; } = null!;      // e.g., "My Shop Address"
        public double PickupLat { get; set; }
        public double PickupLng { get; set; }

        // Is this urgent
        public string Priority { get; set; } = "Normal"; // "Normal" or "Urgent"

        // Packages list (drop-off info)
        public List<CreatePackageDTO> Packages { get; set; } = new List<CreatePackageDTO>();
    }
}
