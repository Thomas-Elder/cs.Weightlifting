using Microsoft.EntityFrameworkCore;

using API.DTOs.Coaches;
using API.Data.Managers.Interfaces;

namespace API.Data.Managers
{
    public class CoachesManager : ICoachesManager
    {
        private readonly WeightliftingContext _weightliftingContext;
        public CoachesManager(WeightliftingContext weightliftingContext)
        {
            _weightliftingContext = weightliftingContext;
        }

        /// <summary>
        /// Gets the coach id for the Application User Id passed in.
        /// If there is an Coach with that Application User Id it sets coachId to  
        /// the value of Coach.Id, and returns true.
        /// 
        /// If there is not an Coach with that Application User Id, coachId is 0,
        /// and the method returns false.
        /// </summary>
        /// <param name="applicationUserId"></param>
        /// <param name="coachId"></param>
        /// <returns>bool</returns>
        public bool GetCoachId(string applicationUserId, out int coachId)
        {
            var coach = _weightliftingContext.Coaches.FirstOrDefault(c => c.ApplicationUserId == applicationUserId);

            if (coach == null)
            {
                coachId = 0;
                return false;
            }

            coachId = coach.Id;
            return true;
        }

        /// <summary>
        /// Adds the Athlete with the given athleteId to the Coach with the given coachId. 
        /// </summary>
        /// If the coachId does not exist, returns a AddAthleteToCoachResponseDTO with Success false.
        /// If the athleteId does not exist, returns a AddAthleteToCoachResponseDTO with Success false.
        /// If both ids exist, but the athlete is already assigned a coach, believe it or not, 
        /// returns a AddAthleteToCoachResponseDTO with Success false.
        /// If both ids exist, and the athlete is not already associated with coach, returns a 
        /// AddAthleteToCoachResponseDTO with Success true.
        /// <param name="coachId"></param>
        /// <param name="athleteId"></param>
        /// <returns></returns>
        public async Task<AddAthleteToCoachResponseDTO> AddAthleteToCoach(int coachId, int athleteId)
        {
            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.Id == coachId);

            if (coach is null)
            {
                return new AddAthleteToCoachResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Coach Id Error", "Coach id doesn't exist" }
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
                        { "Athlete Id Error", "Athlete id doesn't exist" }
                    }
                };
            }

            if (athlete.Coach is not null)
            {
                return new AddAthleteToCoachResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Athlete Error", "Athlete already has a coach" }
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

        public async Task<CoachDetailsResponseDTO> Details(int coachId)
        {
            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.Id == coachId);

            if (coach is null)
            {
                return new CoachDetailsResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Coach Id", "No coach with that id exists" }
                    }
                };
            }

            var athletes = await _weightliftingContext.Athletes
                .Where(a => a.CoachId == coach.Id)
                .ToListAsync();

            var athleteDetailsDTOs = new List<AthleteDetailsDTO>();

            foreach (var athlete in athletes)
            {
                athleteDetailsDTOs.Add(new AthleteDetailsDTO()
                {
                    AthleteId = athlete.Id,
                    FirstName = athlete.FirstName,
                    LastName = athlete.LastName
                });
            }

            return new CoachDetailsResponseDTO()
            {
                Success = true,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                Athletes = athleteDetailsDTOs
            };
        }

        public async Task<CoachDetailsResponseDTO> EditDetails(int coachId, EditDetailsDTO editDetailsDTO)
        {
            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.Id == coachId);

            if (coach is null)
            {
                return new CoachDetailsResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "User ID", "No coach with that user id exists" }
                    }
                };
            }

            coach.FirstName = editDetailsDTO.FirstName ??= coach.FirstName;
            coach.LastName = editDetailsDTO.LastName ??= coach.LastName;

            _weightliftingContext.SaveChanges();

            var athletes = await _weightliftingContext.Athletes
                .Where(a => a.CoachId == coach.Id)
                .ToListAsync();

            var athleteDetailsDTOs = new List<AthleteDetailsDTO>();

            foreach (var athlete in athletes)
            {
                athleteDetailsDTOs.Add(new AthleteDetailsDTO()
                {
                    AthleteId = athlete.Id,
                    FirstName = athlete.FirstName,
                    LastName = athlete.LastName
                });
            }

            return new CoachDetailsResponseDTO()
            {
                Success = true,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                Athletes = athleteDetailsDTOs
            };
        }
    }
}
