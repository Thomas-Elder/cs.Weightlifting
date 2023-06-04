using DTO.Exercises;
using System.ComponentModel.DataAnnotations;

namespace DTO.Sessions
{
    public class AddSessionDTO
    {
        [Required]
        public DateTime Date { get; set; }

        public IEnumerable<ExerciseDTO>? Exercises { get; set; }
    }

    public class AddSessionResponseDTO : ResponseDTO
    {
    }
}
