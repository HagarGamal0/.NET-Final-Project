using System;
namespace PickGo_backend.Models
{

	public class Payment
	{
		public int PaymentId { get; set; }
		public string Method { get; set; }
		public string Status { get; set; }
		public DateTime PaymentDate { get; set; }
		public int InvoiceId { get; set; }
		public Invoice Invoice { get; set; }

	}
}