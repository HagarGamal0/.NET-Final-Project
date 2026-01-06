namespace PickGo_backend.DTOs.Courier
{
    public class UpdateCourierProfileDTO
    {
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string VehicleType { get; set; }
        public string? LicenseNumber { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsOnline { get; set; }

        // الصور (FILES)
        public IFormFile? Photo { get; set; }
        public IFormFile? LicensePhotoFront { get; set; }
        public IFormFile? LicensePhotoBack { get; set; }
        public IFormFile? VehicleLicensePhotoFront { get; set; }
        public IFormFile? VehicleLicensePhotoBack { get; set; }
        public IFormFile? IdPhoto { get; set; }
    }
}
