using System;
using System.Collections.Generic;
 

    namespace PickGo_backend.Models { 


    public class Request : BaseModel
	{



        public DateTime CreatedAt { get; set; }
        public string Source { get; set; } = null!;

        public double Puck_up_lat { get; set; }
        public double Puck_up_lang { get; set; }

        public  int  SupplierId { get; set; }

        public virtual  Supplier Supplier { get; set; } = null!;
        public virtual ICollection<Package>? Packages { get; set; }

        public RequestStatus Status { get; set; }
        = RequestStatus.Pending; //passed on courrier accepntence
    }




}
