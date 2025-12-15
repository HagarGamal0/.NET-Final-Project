using System;
using System.Collections.Generic;
 

    namespace PickGo_backend.Models { 


    public class Request : BaseModel
	{



        public DateTime CreatedAt { get; set; }
        public string Source { get; set; } = null!;

        public double PickupLat { get; set; }
        public double PickupLng { get; set; }

        public  int  SupplierId { get; set; }

        public virtual  Supplier Supplier { get; set; } = null!;
        public virtual ICollection<Package>? Packages { get; set; }

        public RequestStatus Status { get; set; }
        = RequestStatus.Pending; //passed on courrier accepntence
        public bool IsUrgent { get; set; } = false;

    }




}
