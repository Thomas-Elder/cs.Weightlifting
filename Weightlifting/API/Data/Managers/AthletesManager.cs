using Microsoft.EntityFrameworkCore;

using API.DTOs.Coaches;
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

        public async Task<AddAthleteToCoachResponseDTO> AddAthleteToCoach(string athleteUserId, int coachId)
        {
            var athlete = await _weightliftingContext.Athletes.FirstOrDefaultAsync(a => a.ApplicationUserId == athleteUserId);

            if (athlete is null)
            {
                return new AddAthleteToCoachResponseDTO()
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
                return new AddAthleteToCoachResponseDTO()
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

            return new AddAthleteToCoachResponseDTO()
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
    }
}
