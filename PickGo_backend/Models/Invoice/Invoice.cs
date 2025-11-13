
using PickGo_backend.Models.Enums;
using System;

namespace PickGo_backend.Models
{

    public class Invoice : BaseModel
    {
        public double Cost { get; set; }
        public PaymentTypes PaymentType { get; set; }
     
        public string InvoiceNumber { get; set; }



    public Request Request { get; set; }

        public int RequestID { get; set;}

    }
}