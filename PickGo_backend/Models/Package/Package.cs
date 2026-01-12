using System.ComponentModel.DataAnnotations;
using PickGo_backend.Models;

public class Package:BaseModel
{
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public float Weight { get; set; }
    public bool Fragile { get; set; }
    public DateTime ExpireDate { get; set; }
    public float ShipmentCost { get; set; }
    public DateTime? DeliveredAt { get; set; }
    [Required]
    public string? Destination { get; set; }
    public double? Lat { get; set; }

    public double? Lang { get; set; }



    public PackageStatus Status { get; set; } 
    public float ShipmentRating { get; set; }

    public string? ShipmentNotes { get; set; }

    public int RequestID { get; set; }
    public int? CourierID { get; set; }
    public ICollection<Dispute> Disputes { get; set; } = new List<Dispute>();

    public virtual Request Request { get; set; } = null!;
    public int? CustomerID { get; set; }
    public string ReceiverPhone { get; set; } = null!;
    public virtual Customer? Customer { get; set; }
    public virtual Courier? Courier { get; set; }
    public virtual Invoice? Invoice { get; set; }
    public virtual DeliveryProof? DeliveryProof { get; set; }
    public ICollection<CourierTransaction>? Transactions { get; set; }

    public int ShipmentReviewID { get; set; }
    public virtual ShipmentReview? ShipmentReview { get; set; }

    public string? DeliveryOTP { get; set; }
    public bool OTPVerified { get; set; } = false; 


}
