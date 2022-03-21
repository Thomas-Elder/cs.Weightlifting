using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Sessions
{
    public class AddSessionDTO
    {
        [Required]
        public int AthleteId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }

    public class AddSessionResponseDTO : ResponseDTO
    {
    }
}
