
namespace DTO.Sessions
{
    public class GetSessionsDTO
    {
    }

    public class GetSessionsResponseDTO : ResponseDTO
    { 
        public IEnumerable<SessionDetailsDTO>? SessionDetails { get; set; }
    }
}
