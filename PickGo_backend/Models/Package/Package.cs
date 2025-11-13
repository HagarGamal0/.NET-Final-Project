using System;
namespace PickGo_backend.Models
{

	public class Package:BaseModel
	{
		public string Description { get; set; }
		public double Weight { get; set; }
        public bool Fragile { get; set; }
        public DateTime ExpireDate { get; set; }
        public double ShipmentCost { get; set; }


        public string InvoiveImage { get; set; }


	    public Request Request { get; set; }
		public int RequestID { get; set; }

	}
}