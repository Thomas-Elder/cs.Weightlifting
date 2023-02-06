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
        /// </summary>
        /// If there is an Athlete with that Application User Id it sets athleteId to  
        /// the value of Athlete.Id, and returns true.
        /// If there is not an Athlete with that Application User Id, athleteId is 0,
        /// and the method returns false.
        /// <param name="applicationUserId"></param>
        /// <param name="athleteId"></param>
        /// <returns>
        /// True if the given ApplicationUserId is associated with an existing Athlete in the context, and athleteId is set accordingly
        /// </returns>
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

        /// <summary>
        /// Adds a Coach to an Athlete
        /// </summary>
        /// If the coachId does not exist, returns a AddCoachResponseDTO with Success false.
        /// If the athleteId does not exist, returns a AddCoachResponseDTO with Success false.
        /// If both ids exist, but the athlete is already assigned a coach, believe it or not, 
        /// returns a AddCoachResponseDTO with Success false.
        /// If both ids exist, and the athlete is not already associated with coach, returns a 
        /// AddCoachResponseDTO with Success true.
        /// <param name="athleteId"></param>
        /// <param name="coachId"></param>
        /// <returns>
        /// An AddCoachResponseDTO with the result of the action.
        /// </returns>
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

            if (athlete.Coach is not null)
            {
                return new AddCoachResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Athlete", "Athlete already has a coach" }
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

        /// <summary>
        /// Returns the details of the Athlete.
        /// </summary>
        /// Returns an AthleteDetailsDTO.
        /// This will have Success set to false if the athleteId is not found in the database. 
        /// Otherwise it will have Success set to true, and the athlete's details.
        /// <param name="athleteId"></param>
        /// <returns>
        /// An AthleteDetailsDTO with the result of the action.
        /// </returns>
        public async Task<AthleteDetailsDTO> Details(int athleteId)
        {
            var athlete = await _weightliftingContext.Athletes
                .Include(a => a.Sessions)
                .Include(a => a.Coach)
                .Where(a => a.Id == athleteId)
                .SingleOrDefaultAsync();

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

          var sessions = athlete.Sessions;

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

          var coach = athlete.Coach;

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

        /// <summary>
        /// Updates the Athlete's details and returns the updated details.
        /// </summary>
        /// Returns an AthleteDetailsDTO.
        /// This will have Success set to false if the athleteId is not found in the database. 
        /// Otherwise it will have Success set to true, and the athlete's new details.
        /// <param name="athleteId"></param>
        /// <param name="editDetailsDTO"></param>
        /// <returns>
        /// An AthleteDetailsDTO with the result of the action.
        /// </returns>
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

            athlete.FirstName = editDetailsDTO.FirstName ?? athlete.FirstName;
            athlete.LastName = editDetailsDTO.LastName ?? athlete.LastName;

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

        /// <summary>
        /// Deletes an Athlete from the database.
        /// </summary>
        /// Returns a DeleteAthleteDTO with Success flag set to false if the given athleteId is not
        /// associated with an Athlete in the db.
        /// Otherwise it will have Success set to true.
        /// <param name="athleteId"></param>
        /// <returns>
        /// A DeleteAthleteDTO with the result of the action.
        /// </returns>
        public async Task<DeleteAthleteDTO> Delete(int athleteId)
        {
            var athlete = await _weightliftingContext.Athletes.FirstOrDefaultAsync(a => a.Id == athleteId);

            if (athlete is null)
            {
                return new DeleteAthleteDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "User ID", "No athlete with that user id exists" }
                    }
                };
            }

            // else delete and return
            _weightliftingContext.Athletes.Remove(athlete);
            _weightliftingContext.SaveChanges();

            return new DeleteAthleteDTO()
            {
                Success = true
            };
        }
    }
}
