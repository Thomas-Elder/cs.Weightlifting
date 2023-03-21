
using System.ComponentModel.DataAnnotations;

namespace WEB.ViewModels.Account
{
    public class RegisterAthlete
    {
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        [MaxLength(256), EmailAddress, Required(ErrorMessage = "Email required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
