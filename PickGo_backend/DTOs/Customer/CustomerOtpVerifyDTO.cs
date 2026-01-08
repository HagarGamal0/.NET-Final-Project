namespace PickGo_backend.DTOs.Customer
{
    public class CustomerOtpVerifyDTO
    {
        public string PhoneNumber { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
