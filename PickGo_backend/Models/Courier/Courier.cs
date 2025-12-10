
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
        public string PhotoUrl { get; set; }
        public string LicensePhotoFront { get; set; }
        public string LicensePhotoBack { get; set; }
        public string VehcelLicensePhotoBack { get; set; }
        public string VehcelLicensePhotoFront { get; set; }


        public User? User { get; set; }
        public ICollection<CourierLocation> Locations { get; set; }

        public ICollection<Package>? Packages { get; set; }
        public ICollection<DeliveryProof>? DeliveryProofs { get; set; }
        public ICollection<CourierTransaction>? Transactions { get; set; }
        public ICollection<CourierSubscription>? CourierSubscriptions { get; set; } = new List<CourierSubscription>();

    }
}
