using DTO.Athletes;

namespace API.Data.Managers.Interfaces
{
    public interface IAthletesManager
    {
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
        public bool GetAthleteId(string applicationUserId, out int athleteId);

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
        public Task<AddCoachResponseDTO> AddCoach(int athleteId, int coachId);

        /// <summary>
        /// Returns a list of available Coaches.
        /// </summary>
        /// <returns>
        /// An IEnumerable of CoachDetailsDTOs
        /// </returns>
        public Task<GetCoachesDTO> GetCoaches();

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
        public Task<AthleteDetailsDTO> Details(int athleteId);

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
        public Task<AthleteDetailsDTO> EditDetails(int athleteId, EditDetailsDTO editDetailsDTO);

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
        public Task<DeleteAthleteDTO> Delete(int athleteId);
    }
}
