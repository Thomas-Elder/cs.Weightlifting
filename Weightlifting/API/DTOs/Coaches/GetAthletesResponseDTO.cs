using API.Data.Models;

namespace API.DTOs.Coaches
{
    public class GetAthletesResponseDTO
    {
        public bool Success { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
        public Dictionary<int, string>? Result { get; set; }
    }
}
