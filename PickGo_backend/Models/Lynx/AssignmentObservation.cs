using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickGo_backend.Models.Lynx
{
    public class AssignmentObservation
    {
        [Key]
        public int Id { get; set; }

        public int RequestId { get; set; }
        
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }

        public int? CourierId { get; set; }

        // Let's check Courier.cs.
        // In SupplierController: var courier = ... c.Id == dto.CourierId. 
        
        public string Explanation { get; set; }

        public string DecisionSource { get; set; } // SYSTEM, SUPPLIER, ADMIN

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
