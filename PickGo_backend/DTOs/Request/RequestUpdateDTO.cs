namespace PickGo_backend.DTOs.Request
{
    public class RequestUpdateDTO
    {
        public string? Source { get; set; }
        public double? PickupLat { get; set; }
        public double? PickupLng { get; set; }
        public int? SupplierId { get; set; }
        public RequestStatus? Status { get; set; }
    }
}
