namespace PickGo_backend.Models
{


    public abstract class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }


}