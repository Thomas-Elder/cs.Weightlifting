using API.DTOs.Exercises;

namespace API.DTOs.Sessions
{
    public class SessionDetailsDTO : ResponseDTO
    {
        public int SessionId { get; set; }
        public DateTime Date { get; set; }

        public IEnumerable<ExerciseDTO>? Exercises { get; set; }
    }
}
