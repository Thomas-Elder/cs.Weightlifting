using API.DTOs.Coaches;

namespace API.Data.Managers.Interfaces
{
    public interface ICoachesManager
    {
        /// <summary>
        /// Gets the coach id for the Application User Id passed in
        /// </summary>
        /// <param name="applicationUserId"></param>
        /// <param name="coachId"></param>
        /// <returns>
        /// True if the given coachId exists in the context, and coachId is set accordingly
        /// </returns>
        public bool GetCoachId(string applicationUserId, out int coachId);

        /// <summary>
        /// Adds the Athlete with the given athleteId to the Coach with the given coachId. 
        /// </summary>
        /// <param name="coachId"></param>
        /// <param name="athleteId"></param>
        /// <returns>
        /// An AddAthleteToCoachResponseDTO with the result of the action.
        /// </returns>
        public Task<AddAthleteToCoachResponseDTO> AddAthleteToCoach(int coachId, int athleteId);

        /// <summary>
        /// Gets the Coach details associated with the given coachId.
        /// </summary>
        /// <param name="coachId"></param>
        /// <returns>
        /// A CoachDetailsResponseDTO with the result of the action.
        /// </returns>
        public Task<CoachDetailsResponseDTO> Details(int coachId);

        /// <summary>
        /// Updates the details of a Coach record.
        /// </summary>
        /// <param name="coachId"></param>
        /// <param name="editDetailsDTO"></param>
        /// <returns>
        /// A CoachDetailsResponseDTO with the result of the action.
        /// </returns>
        public Task<CoachDetailsResponseDTO> EditDetails(int coachId, EditDetailsDTO editDetailsDTO);
    }
}
