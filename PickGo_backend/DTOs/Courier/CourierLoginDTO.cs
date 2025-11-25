using System.ComponentModel.DataAnnotations;

namespace PickGo_backend.DTOs.Courier
{
    public class CourierLoginDTO
    {

        [Required(ErrorMessage = "Please enter your username or email.")]
        [Display(Name = "Username or Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
