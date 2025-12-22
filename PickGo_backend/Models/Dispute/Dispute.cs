namespace PickGo_backend.Models
{
    public class Dispute : BaseModel
    {
        public int PackageId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Pending"; // Pending / Resolved / Reassigned
        public string ResolutionNotes { get; set; }
        public string DisputeType { get; set; } // Customer / Supplier / Carrier

        // Proof images stored as URLs
        public List<DisputeProof> ProofImages { get; set; } = new();

        // Status history
        public List<DisputeStatusHistory> StatusHistory { get; set; } = new();

        // Navigation
        public virtual Package Package { get; set; }

    }
}
