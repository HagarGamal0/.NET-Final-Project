
using System;

namespace PickGo_backend.Models
{

    public class Invoice : BaseModel
    {
        public int InvoiceId { get; set; }
        public double Amount { get; set; }
        public DateTime IssuanceDate { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public Payment Payment { get; set; }

    }
}