namespace API.DTOs.Sessions
{
    public class EditSessionDetailsDTO
    {
        public int SessionId { get; set; }
        public DateTime Date { get; set; }
    }

    public class EditSessionDetailsResponseDTO : ResponseDTO
    {
        public int SessionId { get; set; }
        public DateTime Date { get; set; }
    }
}
