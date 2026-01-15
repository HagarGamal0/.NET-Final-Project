
using PickGo_backend.Models.Enums;

namespace PickGo_backend.Models
{
    public class Courier : BaseModel
    {
        public string UserId { get; set; } = null!;
        public VehicleType VehicleType { get; set; }
        public string? LicenseNumber { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public bool IsOnline { get; set; }
        public float Rating { get; set; }
        public float? MaxWeight { get; set; }
        public string? PhotoUrl { get; set; }
        
        public string? address { get; set; }
        public string? LicensePhotoFront { get; set; }
        public string? LicensePhotoBack { get; set; }
        public string? VehcelLicensePhotoBack { get; set; }

        public string? VehcelLicensePhotoFront { get; set; }

        public int CompletedDeliveries { get; set; } // Courier.CompletedDeliveries


        public CourierStatus Status { get; set; }
        public string? RejectionReason { get; set; }


        public virtual User User { get; set; }

        public virtual ICollection<CourierLocation> Locations { get; set; }


        //   public int? CurrentPackageId { get; set; } // optional, active package
        // public Package? CurrentPackage { get; set; } // optional

        public int? CurrentSubscriptionId { get; set; }
        public CourierSubscription? CurrentSubscription { get; set; }
        public virtual ICollection<Package>? Packages { get; set; }
        public virtual ICollection<DeliveryProof>? DeliveryProofs { get; set; }
        public virtual ICollection<CourierTransaction>? Transactions { get; set; }
        public virtual ICollection<CourierSubscription> CourierSubscriptions { get; set; } = new List<CourierSubscription>();
        public string IdPhotoUrl { get;  set; }
    }
}
