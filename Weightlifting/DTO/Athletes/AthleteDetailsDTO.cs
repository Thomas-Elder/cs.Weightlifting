namespace DTO.Athletes
{
    public class AthleteDetailsDTO : ResponseDTO
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public IEnumerable<SessionDetailsDTO>? Sessions { get; set; }

        public CoachDetailsDTO? Coach { get; set; }
    }
}
