using System;
using System.Collections.Generic;

namespace PickGo_backend.Models
{

    public class Shipment : BaseModel
    {
        public int ShipmentId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public Order Order { get; set; }
        public List<Package> Packages { get; set; } = new List<Package>();
        public Invoice Invoice { get; set; }


        public int CourierId { get; set; }
        public User Courier { get; set; }
    }

}