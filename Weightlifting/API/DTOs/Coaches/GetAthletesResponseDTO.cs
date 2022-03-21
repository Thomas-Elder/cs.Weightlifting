using API.Data.Models;

namespace API.DTOs.Coaches
{
    public class GetAthletesResponseDTO : ResponseDTO
    {
        public Dictionary<int, string>? Result { get; set; }
    }
}
