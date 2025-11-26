using System.Collections.Generic;

namespace PickGo_backend.Models
{
    public class Subscription : BaseModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public string UserType { get; set; } = null!; // "Courier" أو "Supplier"
        public int MaxOrders { get; set; }

        // علاقات many-to-many
        public ICollection<CourierSubscription> CourierSubscriptions { get; set; } = new List<CourierSubscription>();
        public ICollection<SupplierSubscription> SupplierSubscriptions { get; set; } = new List<SupplierSubscription>();
    }
}