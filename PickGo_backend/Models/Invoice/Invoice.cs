
using PickGo_backend.Models.Enums;
using System;

namespace PickGo_backend.Models
{

    public class Invoice : BaseModel
    {
        public int Id { get; set; }
        public float Cost { get; set; }
        public string? PaymentType { get; set; } 
        public string? InvoiceNumber { get; set; } 
        public int PackageID { get; set; }

        public Package? Package { get; set; }

    }
}