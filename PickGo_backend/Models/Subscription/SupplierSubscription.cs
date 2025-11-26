namespace PickGo_backend.Models
{
    public class SupplierSubscription:BaseModel
    {
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;
    }
}
