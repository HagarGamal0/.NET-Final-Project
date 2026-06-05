using System.ComponentModel.DataAnnotations;

namespace PickGo_backend.DTOs.Supplier

{
    public class SupplierRegisterDTO
    {
    
            public string UserName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;

            // Supplier-specific fields
            public string ShopName { get; set; } = null!;
            public string? Address { get; set; }
            public DateTime? BirthDate { get; set; }
            public string? Gender { get; set; }
        }

    }

