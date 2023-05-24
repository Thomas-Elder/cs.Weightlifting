using DTO.Sets;

namespace DTO.Exercises
{
    public class ExerciseDTO
    {
        public string? Name { get; set; }
        public IEnumerable<SetDTO>? Sets { get; set; }
    }
}
