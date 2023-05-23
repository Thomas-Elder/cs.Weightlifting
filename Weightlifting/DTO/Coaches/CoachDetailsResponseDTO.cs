namespace DTO.Coaches
{
    public class CoachDetailsResponseDTO : ResponseDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public IEnumerable<AthleteDetailsDTO>? Athletes { get; set; }
    }
}
