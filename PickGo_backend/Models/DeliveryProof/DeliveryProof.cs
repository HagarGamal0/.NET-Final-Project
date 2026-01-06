using PickGo_backend.Models;

public class DeliveryProof : BaseModel
{
    public int? PackageID { get; set; }
    public string? SignatureUrl { get; set; }
    public string? CustomerOTP { get; set; }
    public DateTime? DeliveredAt { get; set; }

    public int? CustomerID { get; set; }
    public virtual Customer? Customer { get; set; }

    public int? CourierID { get; set; }
    public virtual Courier? Courier { get; set; }

    public virtual Package Package { get; set; } // required one-to-one
    public string? Notes { get; set; }
}
