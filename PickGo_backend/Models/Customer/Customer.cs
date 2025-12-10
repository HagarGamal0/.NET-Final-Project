
namespace PickGo_backend.Models
{
    public class Customer:BaseModel
    {
        public string UserId { get; set; } 
        public string Address { get; set; } = null!;

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Package>? Packages { get; set; }
        public virtual ICollection<DeliveryProof>? DeliveryProofs { get; set; }

    }
}
