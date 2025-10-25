using System;
using System.Collections.Generic;

    namespace PickGo_backend.Models { 


    public class Order : BaseModel
	{
		public int OrderId { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Status { get; set; }
		public int ClientID { get; set; }
		public User Client { get; set; }
		public Address PickupAddress { get; set; }
		public Address DropAddress { get; set; }
		public ICollection<Package> Packages { get; set; }
		public Invoice Invoice { get; set; }

	}
}