using PickGo_backend.Models.Enums;

namespace PickGo_backend.DTOs.Courier
{
    public class CourierProfileDto
    {

        public string Id { get; set; }
        public string Name { get; set; }           // من User.UserName
        public string Email { get; set; }          // من User.Email
        public string Phone { get; set; }          // من User.Phone أو Courier.Phone
        public string VehicleType { get; set; }    // Courier.VehicleType.ToString()
        public string LicenseNumber { get; set; }
        public float Rating { get; set; }          // Courier.Rating
        public int CompletedDeliveries { get; set; } // Courier.CompletedDeliveries
        public bool IsAvailable { get; set; }      // Courier.IsAvailable
        public bool IsOnline { get; set; }         // Courier.IsOnline
        public CourierLocationDto? Locations { get; set; }
    }
}

