using System.ComponentModel.DataAnnotations;

namespace API.Account.DTOs
{
    public class UserAuthenticationDTO
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }

    }
}
