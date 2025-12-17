using PickGo_backend.Models;

public class Package:BaseModel
{
    public string Description { get; set; } = null!;
    public float Weight { get; set; }
    public bool Fragile { get; set; }
    public DateTime ExpireDate { get; set; }
    public float ShipmentCost { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string? Destination { get; set; }
    public double? Lat { get; set; }

    public double? Lang { get; set; }



    public PackageStatus Status { get; set; } 
    public float ShipmentRating { get; set; }

    public string? ShipmentNotes { get; set; }

    public int RequestID { get; set; }
    public int? CourierID { get; set; }

    public virtual Request Request { get; set; } = null!;
    public int CustomerID { get; set; }  // match Customer.Id
    public virtual Customer Customer { get; set; } = null!;
    public virtual Courier? Courier { get; set; }
    public virtual Invoice? Invoice { get; set; }
    public virtual DeliveryProof? DeliveryProof { get; set; }
    public ICollection<CourierTransaction>? Transactions { get; set; }

    public int ShipmentReviewID { get; set; }
    public virtual ShipmentReview? ShipmentReview { get; set; }

   
}
