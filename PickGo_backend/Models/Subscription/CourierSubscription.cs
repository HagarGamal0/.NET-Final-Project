namespace PickGo_backend.Models
{
    public class CourierSubscription:BaseModel
    {
        public int CourierId { get; set; }
        public Courier Courier { get; set; } = null!;

        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;
    }

}
