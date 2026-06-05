using System.ComponentModel.DataAnnotations;

namespace PickGo_backend.DTOs.User
{
    public class UserLoginDTO
    {

        [Required(ErrorMessage = "Please enter your username or email.")]
        [Display(Name = "Username or Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
