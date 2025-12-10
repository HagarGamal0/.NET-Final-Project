
namespace PickGo_backend.Models
{
    public class Courier :BaseModel
    {
        public string UserId { get; set; } = null!;
        public string VehicleType { get; set; } = null!;
        public string LicenseNumber { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public bool IsOnline { get; set; }
        public float Rating { get; set; }
        public float MaxWeight { get; set; }
        public string Status { get; set; }

        public User? User { get; set; }
        public virtual ICollection<CourierLocation> Locations { get; set; }

        public virtual ICollection<Package>? Packages { get; set; }
        public virtual ICollection<DeliveryProof>? DeliveryProofs { get; set; }
        public virtual ICollection<CourierTransaction>? Transactions { get; set; }
        public virtual ICollection<CourierSubscription>? CourierSubscriptions { get; set; } = new List<CourierSubscription>();

    }
}
