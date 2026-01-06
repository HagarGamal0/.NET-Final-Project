using System.Collections.Generic;

namespace PickGo_backend.Models
{
    public class Subscription : BaseModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public string UserType { get; set; } = null!; // "Courier" or "Supplier"
        public int MaxOrders { get; set; }

        // Many-to-many relationships
        public virtual ICollection<CourierSubscription> CourierSubscriptions { get; set; } = new List<CourierSubscription>();
        public virtual ICollection<SupplierSubscription> SupplierSubscriptions { get; set; } = new List<SupplierSubscription>();
    }
}
