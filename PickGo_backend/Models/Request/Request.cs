using System;
using System.Collections.Generic;


namespace PickGo_backend.Models
{


    public class Request : BaseModel
    {
        public DateTime CreatedAt { get; set; }
        public string Source { get; set; } = null!;
        public string? PickupLat { get; set; }
        public string? PickupLng { get; set; }
        public string? DropoffAddress { get; set; }
        public double? DropoffLat { get; set; }
        public double? DropoffLng { get; set; }
        public int SupplierId { get; set; }
        public Customer? Customer { get; set; }
        public Customer? Phone { get; set; }
        public Supplier Supplier { get; set; } = null!;
        public ICollection<Package>? Packages { get; set; }
        public string Status { get; set; } = "Pending";
    }




}
