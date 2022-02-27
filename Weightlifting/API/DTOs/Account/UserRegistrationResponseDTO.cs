namespace API.DTOs.Account
{
    public class UserRegistrationResponseDTO
    {
        public bool isSuccessfulRegistration { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
    }
}
