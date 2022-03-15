using Microsoft.EntityFrameworkCore;

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

        public async Task<CoachDetailsResponseDTO> DetailsByApplicationUserId(string coachUserId)
        {
            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.ApplicationUserId == coachUserId);

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

        public async Task<CoachDetailsResponseDTO> DetailsByCoachId(int coachId)
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

        public async Task<CoachDetailsResponseDTO> EditDetailsByApplicationUserId(string coachUserId, EditDetailsDTO editDetailsDTO)
        {
            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.ApplicationUserId == coachUserId);

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

            coach.FirstName = editDetailsDTO.FirstName is not null ? editDetailsDTO.FirstName : coach.FirstName;
            coach.LastName = editDetailsDTO.LastName is not null ? editDetailsDTO.LastName : coach.LastName;

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

        public async Task<CoachDetailsResponseDTO> EditDetailsByCoachId(int coachId, EditDetailsDTO editDetailsDTO)
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

            coach.FirstName = editDetailsDTO.FirstName is not null ? editDetailsDTO.FirstName : coach.FirstName;
            coach.LastName = editDetailsDTO.LastName is not null ? editDetailsDTO.LastName : coach.LastName;

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
