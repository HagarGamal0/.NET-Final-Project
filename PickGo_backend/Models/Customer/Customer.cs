
namespace PickGo_backend.Models
{
    public class Customer : BaseModel
    {
        public string? UserId { get; set; }
        public string Address { get; set; } = null!;
        public string? Phone { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Package>? Packages { get; set; }
        public ICollection<DeliveryProof>? DeliveryProofs { get; set; }

    }
}
