using Microsoft.EntityFrameworkCore;

using API.Data.Models;
using API.DTOs.Sessions;
using API.Data.Managers.Interfaces;
using API.DTOs.Exercises;
using API.DTOs.Sets;

namespace API.Data.Managers
{
    /// <summary>
    /// A class for managing creation and updating of Sessions.
    /// </summary>
    public class SessionsManager : ISessionsManager
    {
        private readonly WeightliftingContext _weightliftingContext;
        public SessionsManager(WeightliftingContext weightliftingContext)
        {
            _weightliftingContext = weightliftingContext;
        }

        /// <summary>
        /// Adds a Session for an Athlete
        /// </summary>
        /// Checks if the athleteId exists in the database, and returns an AddSessionResponseDTO with Success
        /// set to false if there is no matching athlete, and Error message in the Errors dict.  
        /// If there is, it creates a Session, and adds all the Exercises, Sets and this Session to the database,
        /// before returning an AddSessionResponseDTO with Success set to true.
        /// <param name="addSessionDTO"></param>
        /// <returns>
        /// An AddSessionResponseDTO with the result of the action.
        /// </returns>
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

            var session = new Session()
            {
                Date = addSessionDTO.Date,
                AthleteId = athlete.Id,
                Athlete = athlete
            };

            foreach (ExerciseDTO exerciseDTO in addSessionDTO.Exercises ?? Enumerable.Empty<ExerciseDTO>())
            {
                var exercise = new Exercise()
                {
                    Name = exerciseDTO.Name,
                    Session = session
                };

                foreach (SetDTO setDTO in exerciseDTO.Sets ?? Enumerable.Empty<SetDTO>())
                {
                    var set = new Set()
                    {
                        Weight = setDTO.Weight,
                        SuccessfulRepetitions = setDTO.SuccessfulRepetitions,
                        FailedRepetitions = setDTO.FailedRepetitions,
                        Exercise = exercise
                    };

                    await _weightliftingContext.Sets.AddAsync(set);
                }

                await _weightliftingContext.Exercises.AddAsync(exercise);
            }

            await _weightliftingContext.Sessions.AddAsync(session);

            _weightliftingContext.SaveChanges();

            return new AddSessionResponseDTO()
            {
                Success = true
            };
        }

        /// <summary>
        /// Gets the details for a given sessionId.
        /// </summary>
        /// Checks if the sessionId matches an existing Session in the database, if not, returns a SessionDetailsDTO
        /// with Success set to false, and Error message in the Errors dict.
        /// If it exists, the details of the Session are returned in a SessionDetailsDTO with Success set to true.
        /// <param name="sessionId"></param>
        /// <returns>
        /// A SessionDetailsDTO with the result of the action.
        /// </returns>
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

            var exercises = await _weightliftingContext.Exercises.Where(e => e.SessionId == session.Id).ToListAsync();

            var exerciseDTOs = new List<ExerciseDTO>();

            foreach(Exercise exercise in exercises)
            {
                var sets = await _weightliftingContext.Sets.Where(s => s.ExerciseId == exercise.Id).ToListAsync();

                var setDTOs = new List<SetDTO>();

                foreach(Set set in sets)
                {
                    setDTOs.Add(new SetDTO()
                    {
                        Weight = set.Weight,
                        SuccessfulRepetitions = set.SuccessfulRepetitions,
                        FailedRepetitions = set.FailedRepetitions
                    });
                }

                exerciseDTOs.Add(new ExerciseDTO()
                {
                    Name = exercise.Name,
                    Sets = setDTOs
                });
            }

            return new SessionDetailsDTO()
            {
                Success = true,
                SessionId = session.Id,
                Date = session.Date,
                Exercises = exerciseDTOs
            };
        }

        /// <summary>
        /// Updates the details of a Session.
        /// </summary>
        /// Checks if the sessionId matches an existing Session in the database, if not, returns a SessionDetailsDTO
        /// with Success set to false, and Error message in the Errors dict.
        /// If the Session exists, the details of the Session are updated, and the updated details are returned in a 
        /// editSessionDetailsDTO, with Success set to true.
        /// <param name="editSessionDetailsDTO"></param>
        /// <returns>
        /// A EditSessionDetailsDTO with the result of the action. 
        /// </returns>
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
