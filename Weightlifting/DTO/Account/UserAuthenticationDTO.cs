using System.ComponentModel.DataAnnotations;

namespace DTO.Account
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
