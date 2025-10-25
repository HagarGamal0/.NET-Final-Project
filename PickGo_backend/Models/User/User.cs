using System;
namespace PickGo_backend.Models
{

    public class User : BaseModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Location { get; set; }
        public DateTime BirthDate { get; set; }
    }
}