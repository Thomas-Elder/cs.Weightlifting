namespace DTO.Athletes
{
    public class GetCoachesDTO : ResponseDTO
    {
        public IEnumerable<CoachDetailsDTO>? Coaches { get; set; }
    }
}
