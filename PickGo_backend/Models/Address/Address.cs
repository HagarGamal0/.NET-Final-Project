using System;
namespace PickGo_backend.Models
{

    public class Address
    {
        public int AddressId { get; set; }
        public string City { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
