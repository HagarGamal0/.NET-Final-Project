using PickGo_backend.Models;

public class ShipmentReview : BaseModel
{
    public float Rating { get; set; }
    public string? ReviewText { get; set; }

    // Foreign Key
    public int PackageID { get; set; }

    // Navigation
    public virtual Package Package { get; set; }
}
