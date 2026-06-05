namespace PickGo_backend.Models
{
    public class DisputeStatusHistory:BaseModel
    {
        public int DisputeId { get; set; }
        public string Status { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public virtual Dispute Dispute { get; set; }
    }
}
