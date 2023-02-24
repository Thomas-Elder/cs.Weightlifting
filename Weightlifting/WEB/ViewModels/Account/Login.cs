
using System.ComponentModel.DataAnnotations;

namespace WEB.ViewModels.Account
{
    public class Login
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
