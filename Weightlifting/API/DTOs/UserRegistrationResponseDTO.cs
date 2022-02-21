namespace API.DTOs
{
    public class UserRegistrationResponseDTO
    {
        public bool isSuccessfulRegistration { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
    }
}
