using System;
using System.Collections.Generic;



namespace PickGo_backend.Models

{
    public class Request : BaseModel
    {
        public DateTime CreatedAt { get; set; }

        public string PickupAddress { get; set; }
        public double? PickupLat { get; set; }
        public double? PickupLng { get; set; }

        public string DropoffAddress { get; set; }
        public double? DropoffLat { get; set; }
        public double? DropoffLng { get; set; }

        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }

        public string ItemsDescription { get; set; }
        public double CODAmount { get; set; }

        public string Notes { get; set; }

        public string Status { get; set; } = "Pending";

        public string UserID { get; set; }
        public ICollection<Package>? Packages { get; set; }
    }
}
