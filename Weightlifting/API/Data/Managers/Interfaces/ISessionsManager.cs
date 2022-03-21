using API.DTOs.Sessions;

namespace API.Data.Managers.Interfaces
{
    public interface ISessionsManager
    {
        public Task<AddSessionResponseDTO> AddSession(AddSessionDTO addSessionDTO);
        public Task<SessionDetailsDTO> Details(int sessionId);
        public Task<EditSessionDetailsResponseDTO> EditDetails(EditSessionDetailsDTO editSessionDetailsDTO);
    }
}
