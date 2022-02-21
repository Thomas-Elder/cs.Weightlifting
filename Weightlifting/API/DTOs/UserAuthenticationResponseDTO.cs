namespace API.DTOs
{
    public class UserAuthenticationResponseDTO
    {
        public bool IsSuccess { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
        public string? Token { get; set; }
    }
}
