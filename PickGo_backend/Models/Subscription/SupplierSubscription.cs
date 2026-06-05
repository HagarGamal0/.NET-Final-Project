namespace PickGo_backend.Models
{
    public class SupplierSubscription:BaseModel
    {
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; } = null!;

        public int SubscriptionId { get; set; }
        public virtual Subscription Subscription { get; set; } = null!;
    }
}
