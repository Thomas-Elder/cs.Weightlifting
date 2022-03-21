namespace API.DTOs.Athletes
{
    public class EditDetailsDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class EditDetailsResponseDTO : ResponseDTO
    {
    }
}
