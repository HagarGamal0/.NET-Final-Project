namespace PickGo_backend.DTOs.DeliveryProof
{
    public class DeliverPackageDto
    {

        public string? CustomerOTP { get; set; }
        public string? Notes { get; set; }          // Optional, any shipment notes
    }
}
