
namespace PickGo_backend.DTOs.Supplier
{
    public class SupplierProfileDTO
    {
        public int Id { get; set; }
        public string ShopName { get; set; } = null!;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string UserId { get; set; } = null!;
    }
}
