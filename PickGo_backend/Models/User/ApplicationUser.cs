using Microsoft.AspNetCore.Identity;

namespace PickGo_backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }


    }
}
