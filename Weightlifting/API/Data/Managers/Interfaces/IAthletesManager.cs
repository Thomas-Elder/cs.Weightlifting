using API.DTOs.Athletes;

namespace API.Data.Managers.Interfaces
{
    public interface IAthletesManager
    {
        /// <summary>
        /// Gets the athlete id for the Application User Id passed in.
        /// </summary>
        /// <param name="applicationUserId"></param>
        /// <param name="athleteId"></param>
        /// <returns>
        /// True if the given ApplicationUserId is associated with an existing Athlete in the context, and athleteId is set accordingly
        /// </returns>
        public bool GetAthleteId(string applicationUserId, out int athleteId);

        /// <summary>
        /// Adds a Coach to an Athlete
        /// </summary>
        /// <param name="athleteId"></param>
        /// <param name="coachId"></param>
        /// <returns>
        /// An AddCoachResponseDTO with the result of the action.
        /// </returns>
        public Task<AddCoachResponseDTO> AddCoach(int athleteId, int coachId);

        /// <summary>
        /// Returns the details of the Athlete.
        /// </summary>
        /// <param name="athleteId"></param>
        /// <returns>
        /// An AthleteDetailsDTO with the result of the action.
        /// </returns>
        public Task<AthleteDetailsDTO> Details(int athleteId);

        /// <summary>
        /// Updates the Athlete's details and returns the updated details.
        /// </summary>
        /// <param name="athleteId"></param>
        /// <param name="editDetailsDTO"></param>
        /// <returns>
        /// An AthleteDetailsDTO with the result of the action.
        /// </returns>
        public Task<AthleteDetailsDTO> EditDetails(int athleteId, EditDetailsDTO editDetailsDTO);
    }
}
