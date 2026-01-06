namespace PickGo_backend.Models
{
    public class CourierTransaction:BaseModel
    {

        public float Amount { get; set; }
        public string Type { get; set; } = null!;
        public DateTime CreatedAt { get; set; }



        public int CourierID { get; set; }
        public virtual Courier Courier { get; set; }

        public int PackageID { get; set; }
        public virtual Package Package { get; set; }
    }
}
