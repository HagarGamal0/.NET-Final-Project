namespace PickGo_backend.DTOs.Request
{
    public class RequestCreateDTO
    {
        public string PickupAddress { get; set; }
        public double? PickupLat { get; set; }
        public double? PickupLng { get; set; }

        public string DropoffAddress { get; set; }
        public double? DropoffLat { get; set; }
        public double? DropoffLng { get; set; }

        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }

        public string ItemsDescription { get; set; }
        public double CODAmount { get; set; }
        public string Notes { get; set; }
    }
}
