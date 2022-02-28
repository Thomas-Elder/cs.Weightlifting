namespace API.DTOs.Coaches
{
    public class AddAthleteToCoachResponseDTO
    {
        public bool Success { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
    }
}
