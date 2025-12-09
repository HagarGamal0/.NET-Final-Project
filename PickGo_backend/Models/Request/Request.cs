using System;
using System.Collections.Generic;
 

    namespace PickGo_backend.Models { 


    public class Request : BaseModel
	{
        public DateTime CreatedAt { get; set; }
        public string Source { get; set; } = null!;
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; } = null!;
        public ICollection<Package>? Packages { get; set; }
    }




}
