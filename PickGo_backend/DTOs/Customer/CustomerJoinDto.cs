using System.ComponentModel.DataAnnotations;

namespace PickGo_backend.DTOs.Customer
{
    public class CustomerJoinDto
    {
        public string? Address { get; set; } // optional, will be stored in User

        [Required]
        public string PhoneNumber { get; set; } = null!;
        public string? UserName { get; set; } // optional username
        public string? Email { get; set; } // optional username

        public string? UserId { get; set; }  // optional

    }
}
