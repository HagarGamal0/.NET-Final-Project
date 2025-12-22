namespace PickGo_backend.Models
{
    public class DisputeStatusHistory:BaseModel
    {
        public int DisputeId { get; set; }
        public string Status { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public Dispute Dispute { get; set; }
    }
}
