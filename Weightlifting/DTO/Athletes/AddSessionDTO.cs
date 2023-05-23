using System.ComponentModel.DataAnnotations;

namespace DTO.Athletes
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
