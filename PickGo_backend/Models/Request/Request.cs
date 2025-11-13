using System;
using System.Collections.Generic;
 

    namespace PickGo_backend.Models { 


    public class Request : BaseModel
	{
		public DateTime CreatedAt { get; set; }
		public string Source { get; set; }
		public string Destination { get; set; }
        public string ShipmentCost { get; set; }

        public User User { get; set; }

		public string UserID { get; set; }




	}
}