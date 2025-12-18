namespace PickGo_backend.DTOs.Customer
{
    public class CustomerRateDto
    {

        public int PackageId { get; set; }          // Package being rated
        public int Rating { get; set; }             // 1-5 stars
        public string? Comment { get; set; }
    }
}
