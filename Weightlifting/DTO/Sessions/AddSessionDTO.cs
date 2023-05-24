using DTO.Exercises;
using System.ComponentModel.DataAnnotations;

namespace DTO.Sessions
{
    public class AddSessionDTO
    {
        [Required]
        public int AthleteId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public IEnumerable<ExerciseDTO>? Exercises { get; set; }
    }

    public class AddSessionResponseDTO : ResponseDTO
    {
    }
}
