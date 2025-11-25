using Microsoft.AspNetCore.Identity;

namespace PickGo_backend.Models
{
    public class User : IdentityUser
    {
        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }

        public ICollection<IdentityUserRole<string>>? UserRoles { get; set; }

        public Courier? Courier { get; set; } // optional navigation
        public Supplier? Supplier { get; set; } // optional navigation

        public Customer? Customer { get; set; } // optional navigation






    }
}