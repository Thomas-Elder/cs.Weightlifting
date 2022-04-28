using Microsoft.EntityFrameworkCore;

using API.DTOs.Athletes;
using API.Data.Managers.Interfaces;
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

        /// <summary>
        /// Gets the athlete id for the Application User Id passed in.
        /// If there is an Athlete with that Application User Id it sets athleteId to  
        /// the value of Athlete.Id, and returns true.
        /// 
        /// If there is not an Athlete with that Application User Id, athleteId is 0,
        /// and the method returns false.
        /// </summary>
        /// <param name="applicationUserId"></param>
        /// <param name="athleteId"></param>
        /// <returns></returns>
        public bool GetAthleteId(string applicationUserId, out int athleteId)
        {
            var athlete = _weightliftingContext.Athletes.FirstOrDefault(a => a.ApplicationUserId == applicationUserId);

            if (athlete is null)
            {
                athleteId = 0;
                return false;
            }

            athleteId = athlete.Id;
            return true;
        }

        public async Task<AddCoachResponseDTO> AddCoach(int athleteId, int coachId)
        {
            var athlete = await _weightliftingContext.Athletes.FirstOrDefaultAsync(a => a.Id == athleteId);

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
        
        public async Task<AthleteDetailsDTO> Details(int athleteId)
        {
            var athlete = await _weightliftingContext.Athletes.FirstOrDefaultAsync(a => a.Id == athleteId);

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

            var athleteDetailsDTO = new AthleteDetailsDTO();

            athleteDetailsDTO.Success = true;
            athleteDetailsDTO.FirstName = athlete.FirstName;
            athleteDetailsDTO.LastName = athlete.LastName;

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

            athleteDetailsDTO.Sessions = sessionDTOs;

            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.Id == athlete.CoachId);

            if (coach is not null)
            {
                athleteDetailsDTO.Coach = new CoachDetailsDTO()
                {
                    CoachId = coach.Id,
                    FirstName = coach.FirstName,
                    LastName = coach.LastName
                };
            }

            return athleteDetailsDTO;
        }

        public async Task<AthleteDetailsDTO> EditDetails(int athleteId, EditDetailsDTO editDetailsDTO)
        {
            var athlete = await _weightliftingContext.Athletes.FirstOrDefaultAsync(a => a.Id == athleteId);

            if (athlete is null)
            {
                return new AthleteDetailsDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "User ID", "No athlete with that user id exists" }
                    }
                };
            }

            athlete.FirstName = editDetailsDTO.FirstName ??= athlete.FirstName;
            athlete.LastName = editDetailsDTO.LastName ??= athlete.LastName;

            _weightliftingContext.SaveChanges();

            var athleteDetailsDTO = new AthleteDetailsDTO();

            athleteDetailsDTO.Success = true;
            athleteDetailsDTO.FirstName = athlete.FirstName;
            athleteDetailsDTO.LastName = athlete.LastName;

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

            athleteDetailsDTO.Sessions = sessionDTOs;

            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.Id == athlete.CoachId);

            if (coach is not null)
            {
                athleteDetailsDTO.Coach = new CoachDetailsDTO()
                {
                    CoachId = coach.Id,
                    FirstName = coach.FirstName,
                    LastName = coach.LastName
                };
            }

            return athleteDetailsDTO;
        }
    }
}
