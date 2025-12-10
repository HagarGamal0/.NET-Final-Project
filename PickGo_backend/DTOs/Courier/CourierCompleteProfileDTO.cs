namespace PickGo_backend.DTOs.Courier
{
    public class CourierCompleteProfileDTO
    {
        public string? VehicleType { get; set; }
        public string? LicenseNumber { get; set; }
        public float? MaxWeight { get; set; }

        // Photos
        public string? PhotoUrl { get; set; }
        public string? LicensePhotoFront { get; set; }
        public string? LicensePhotoBack { get; set; }
        public string? VehcelLicensePhotoBack { get; set; }
        public string? VehcelLicensePhotoFront { get; set; }
    }
}
