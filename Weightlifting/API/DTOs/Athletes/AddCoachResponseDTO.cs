namespace API.DTOs.Athletes
{
    public class AddCoachResponseDTO
    {
        public bool Success { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
    }
}
