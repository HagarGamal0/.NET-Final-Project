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

    public PackageStatus Status { get; set; } 
    public float ShipmentRating { get; set; }
    public float Rating { get; set; }
    public string? ShipmentNotes { get; set; }

    public int RequestID { get; set; }
    public int? CourierID { get; set; }

    public Request Request { get; set; } = null!;
    public int CustomerID { get; set; }  // match Customer.Id
    public Customer Customer { get; set; } = null!;
    public Courier? Courier { get; set; }
    public Invoice? Invoice { get; set; }
    public DeliveryProof? DeliveryProof { get; set; }
    public ICollection<CourierTransaction>? Transactions { get; set; }
}
