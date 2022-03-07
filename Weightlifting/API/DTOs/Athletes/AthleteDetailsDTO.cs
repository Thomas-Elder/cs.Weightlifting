using API.Data.Models;

namespace API.DTOs.Athletes
{
    public class AthleteDetailsDTO
    {
        public bool Success { get; set; }
        public Dictionary<string, string>? Errors { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public IEnumerable<SessionDetailsDTO>? Sessions { get; set; }

        public CoachDetailsDTO? Coach { get; set; }
    }
}
