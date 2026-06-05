namespace PickGo_backend.DTOs.Supplier
{
    public class SupplierCompleteProfileDTO
    {
        public string? ShopName { get; set; }
        public bool? IsDeleted { get; set; }

        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
