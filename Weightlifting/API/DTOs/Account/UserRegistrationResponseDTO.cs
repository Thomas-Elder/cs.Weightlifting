namespace API.DTOs.Account
{
    public class UserRegistrationResponseDTO
    {
        public bool Success { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
    }
}
