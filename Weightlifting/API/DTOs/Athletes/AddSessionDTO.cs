using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Athletes
{
    public class AddSessionDTO
    {
        [Required]
        public DateTime Date { get; set; }
    }

    public class AddSessionResponseDTO : ResponseDTO
    {
    }
}
