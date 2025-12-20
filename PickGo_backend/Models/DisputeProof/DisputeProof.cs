namespace PickGo_backend.Models
{
    public class DisputeProof:BaseModel
    {
        public int DisputeId { get; set; }
        public string ImageUrl { get; set; }
        public virtual Dispute Dispute { get; set; }
    }
}
