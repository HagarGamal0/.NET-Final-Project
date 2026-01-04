using PickGo_backend.Models.Enums;

namespace PickGo_backend.DTOs.Courier
{
    public class CourierCompleteProfileDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }           // User.UserName
        public string Email { get; set; }          // User.Email
        public string Phone { get; set; }          // User.Phone
        public string VehicleType { get; set; }    // VehicleType.ToString()
        public string? LicenseNumber { get; set; }
        public float Rating { get; set; }
        public int CompletedDeliveries { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsOnline { get; set; }

        public string? PhotoUrl { get; set; }
        public string? Address { get; set; }
        public string? LicensePhotoFront { get; set; }
        public string? LicensePhotoBack { get; set; }
        public string? VehicleLicensePhotoFront { get; set; }
        public string? VehicleLicensePhotoBack { get; set; }
        public string? IdPhotoUrl { get; set; }
        public List<CourierLocationDto>? Locations { get; set; }
    }
}
