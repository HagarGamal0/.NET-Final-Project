namespace PickGo_backend.DTOs.User
{
    public class EditProfileDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
    }
}
