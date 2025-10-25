using System;
namespace PickGo_backend.Models
{

	public class Package
	{
		public int PackageId { get; set; }
		public string Description { get; set; }
		public double Weight { get; set; }
		public int OrderId { get; set; }
		public Order Order { get; set; }
	}
}