using Microsoft.EntityFrameworkCore;

using API.Data.Models;
using API.DTOs.Sessions;

namespace API.Data.Managers
{
    public class SessionsManager : ISessionsManager
    {
        private readonly WeightliftingContext _weightliftingContext;
        public SessionsManager(WeightliftingContext weightliftingContext)
        {
            _weightliftingContext = weightliftingContext;
        }

        public async Task<AddSessionResponseDTO> AddSession(AddSessionDTO addSessionDTO)
        {
            var athlete = await _weightliftingContext.Athletes.FirstOrDefaultAsync(a => a.Id == addSessionDTO.AthleteId);

            if (athlete is null)
            {
                return new AddSessionResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Athlete ID", "Athlete id doesn't exist" }
                    }
                };
            }

            await _weightliftingContext.Sessions.AddAsync(new Session()
            {
                Date = addSessionDTO.Date,
                AthleteId = athlete.Id,
                Athlete = athlete
            });

            _weightliftingContext.SaveChanges();

            return new AddSessionResponseDTO()
            {
                Success = true
            };
        }

        public async Task<SessionDetailsDTO> Details(int sessionId)
        {
            var session = await _weightliftingContext.Sessions.FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session is null)
            {
                return new SessionDetailsDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Session Id", "Session id does not exist" }
                    }
                };
            }

            return new SessionDetailsDTO()
            {
                Success = true,
                SessionId = session.Id,
                Date = session.Date
            };
        }

        public async Task<EditSessionDetailsResponseDTO> EditDetails(EditSessionDetailsDTO editSessionDetailsDTO)
        {
            var session = await _weightliftingContext.Sessions.FirstOrDefaultAsync(s => s.Id == editSessionDetailsDTO.SessionId);

            if (session is null)
            {
                return new EditSessionDetailsResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Session Id", "Session id does not exist" }
                    }
                };
            }

            session.Date = editSessionDetailsDTO.Date;

            return new EditSessionDetailsResponseDTO()
            {
                Success = true,
                SessionId = session.Id,
                Date = session.Date
            };
        }
    }
}
