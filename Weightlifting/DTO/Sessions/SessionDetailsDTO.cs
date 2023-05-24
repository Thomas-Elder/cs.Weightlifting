using DTO.Exercises;

namespace DTO.Sessions
{
    public class SessionDetailsDTO : ResponseDTO
    {
        public int SessionId { get; set; }
        public DateTime Date { get; set; }

        public IEnumerable<ExerciseDTO>? Exercises { get; set; }
    }
}
