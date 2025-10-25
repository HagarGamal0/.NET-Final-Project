using Microsoft.AspNetCore.Identity;

namespace PickGo_backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }

    }
}
