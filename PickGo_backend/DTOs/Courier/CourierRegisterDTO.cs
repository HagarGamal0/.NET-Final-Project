using PickGo_backend.Models.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PickGo_backend.DTOs.Courier

{

    public class CourierRegisterDTO
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        // Courier-specific fields
        public VehicleType VehicleType { get; set; }   
        public string LicenseNumber { get; set; } = null!;
        public float MaxWeight { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Status { get; set; }

        public IFormFile? PhotoUrl { get; set; }
        public IFormFile? LicensePhotoFront { get; set; }
        public IFormFile? LicensePhotoBack { get; set; }
        public IFormFile? VehcelLicensePhotoFront { get; set; }
        public IFormFile? VehcelLicensePhotoBack { get; set; }
        public IFormFile? IdPhotoUrl { get; set; }
    }
}
