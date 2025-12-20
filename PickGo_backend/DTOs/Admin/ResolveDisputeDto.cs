namespace PickGo_backend.DTOs
{
    public class ResolveDisputeDto
    {
        public string Status { get; set; } // Resolved / Reassigned / Refunded / Penalized
        public string Notes { get; set; }  // Optional
    }
}
