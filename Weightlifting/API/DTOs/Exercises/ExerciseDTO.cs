using API.DTOs.Sets;

namespace API.DTOs.Exercises
{
    public class ExerciseDTO
    {
        public string? Name { get; set; }
        public IEnumerable<SetDTO>? Sets { get; set; }
    }
}
