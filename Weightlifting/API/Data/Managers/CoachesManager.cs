using Microsoft.EntityFrameworkCore;

using API.Data.Models;
using API.DTOs.Coaches;

namespace API.Data.Managers
{
    public class CoachesManager : ICoachesManager
    {
        private readonly WeightliftingContext _weightliftingContext;
        public CoachesManager(WeightliftingContext weightliftingContext)
        {
            _weightliftingContext = weightliftingContext;
        }

        public async Task<AddAthleteToCoachResponseDTO> AddAthleteToCoach(string coachUserId, int athleteId)
        {
            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.ApplicationUserId == coachUserId);

            if (coach is null)
            {
                return new AddAthleteToCoachResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Coach", "Coach id doesn't exist" }
                    }
                };
            }

            var athlete = await _weightliftingContext.Athletes.FirstOrDefaultAsync(a => a.Id == athleteId);

            if (athlete is null)
            {
                return new AddAthleteToCoachResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Coach", "Athlete id doesn't exist" }
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

        public async Task<GetAthletesResponseDTO> GetAthletes(string id)
        {
             var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.ApplicationUserId == id);

            if (coach is null)
            {
                return new GetAthletesResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Coach", "Coach id doesn't exist" }
                    }
                };
            }

            var result = await _weightliftingContext.Athletes
                .Where(a => a.CoachId == coach.Id)
                .ToDictionaryAsync(a => a.Id, a => a.FirstName);

            if (result is null)
            {
                return new GetAthletesResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Coach", "Coach id has no athletes" }
                    }
                };
            }

            return new GetAthletesResponseDTO()
            {
                Success = true,
                Result = result!
            };
        }
    }
}
