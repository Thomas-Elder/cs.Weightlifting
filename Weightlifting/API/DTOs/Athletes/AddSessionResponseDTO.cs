namespace API.DTOs.Athletes
{
    public class AddSessionResponseDTO
    {
        public bool Success { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
    }
}
