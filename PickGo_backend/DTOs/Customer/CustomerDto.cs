namespace PickGo_backend.DTOs.Customer
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;  // explicitly show phone number
        public string? Address { get; set; }
        public string? UserName { get; set; }  // optional user-defined name

        public int PackagesCount { get; set; }
    }

}
