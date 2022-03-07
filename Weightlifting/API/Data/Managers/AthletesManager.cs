using Microsoft.EntityFrameworkCore;

using API.DTOs.Athletes;
using API.Data.Models;

namespace API.Data.Managers
{
    public class AthletesManager : IAthletesManager
    {
        private readonly WeightliftingContext _weightliftingContext;
        public AthletesManager(WeightliftingContext weightliftingContext)
        {
            _weightliftingContext = weightliftingContext;
        }

        public async Task<AddCoachResponseDTO> AddCoach(string athleteUserId, int coachId)
        {
            var athlete = await _weightliftingContext.Athletes.FirstOrDefaultAsync(a => a.ApplicationUserId == athleteUserId);

            if (athlete is null)
            {
                return new AddCoachResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Athlete ID", "Athlete id doesn't exist" }
                    }
                };
            }

            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.Id == coachId);

            if (coach is null)
            {
                return new AddCoachResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Coach ID", "Coach id doesn't exist" }
                    }
                };
            }

            athlete.CoachId = coach.Id;
            athlete.Coach = coach;

            _weightliftingContext.SaveChanges();

            return new AddCoachResponseDTO()
            {
                Success = true
            };
        }

        public async Task<AddSessionResponseDTO> AddSession(string athleteUserId, AddSessionDTO addSessionDTO)
        {
            var athlete = await _weightliftingContext.Athletes.FirstOrDefaultAsync(a => a.ApplicationUserId == athleteUserId);

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

        public async Task<AthleteDetailsDTO> AthleteDetails(string athleteUserId)
        {
            var athlete = await _weightliftingContext.Athletes.FirstOrDefaultAsync(a => a.ApplicationUserId == athleteUserId);

            if (athlete is null)
            {
                return new AthleteDetailsDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Athlete ID", "Athlete id doesn't exist" }
                    }
                };
            }

            var sessions = await _weightliftingContext.Sessions
                .Where(s => s.AthleteId == athlete.Id)
                .ToListAsync();

            var sessionDTOs = new List<SessionDetailsDTO>();

            foreach (var session in sessions)
            {
                sessionDTOs.Add(new SessionDetailsDTO()
                {
                    SessionId = session.Id,
                    Date = session.Date
                });
            }

            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.Id == athlete.CoachId);

            var coachDetailsDTO = new CoachDetailsDTO()
            {
                CoachId = coach.Id,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
            };

            return new AthleteDetailsDTO()
            {
                Success = true,
                FirstName = athlete.FirstName,
                LastName = athlete.LastName,
                Sessions = sessionDTOs,
                Coach = coachDetailsDTO
            };
        }
    }
}
