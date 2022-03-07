namespace API.DTOs.Athletes
{
    public class EditDetailsResponseDTO
    {
        public bool Success { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
    }
}
