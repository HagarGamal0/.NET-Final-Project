namespace PickGo_backend.Models
{
    public class CourierSubscription : BaseModel
    {
        // Foreign key to Courier
        public int CourierId { get; set; }
        public Courier Courier { get; set; } = null!;

        // Foreign key to Subscription
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;

        // Subscription start and end
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }

        // Is this subscription currently active
        public bool IsActive { get; set; } = true;
    }
}
