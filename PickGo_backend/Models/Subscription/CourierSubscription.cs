namespace PickGo_backend.Models
{
    public class CourierSubscription : BaseModel
    {
        public int CourierId { get; set; }
        public virtual Courier Courier { get; set; } = null!;

        public int SubscriptionId { get; set; }
        public virtual Subscription Subscription { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int UsedTrips { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public string? StripePaymentIntentId { get; set; }
    }
}
