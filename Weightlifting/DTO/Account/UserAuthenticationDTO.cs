using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Account
{
    public class UserAuthenticationDTO
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }

    public class UserAuthenticationResponseDTO : ResponseDTO
    {
        public string? Token { get; set; }
    }
}
