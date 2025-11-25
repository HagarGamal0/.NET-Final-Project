namespace PickGo_backend.Models
{
    public class CourierLocation:BaseModel
    {
        public int CourierID { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public DateTime RecordedAt { get; set; }

        public Courier Courier { get; set; } = null!;
    }
}
