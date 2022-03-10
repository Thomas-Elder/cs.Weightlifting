namespace API.DTOs.Coaches
{
    public class CoachDetailsDTO
    {
        public bool Success { get; set; }
        public Dictionary<string, string>? Errors { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public IEnumerable<AthleteDetailsDTO>? Athletes { get; set; }
    }
}
