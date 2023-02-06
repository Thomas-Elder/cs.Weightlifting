using API.DTOs.Coaches;

namespace API.Data.Managers.Interfaces
{
    public interface ICoachesManager
    {
        /// <summary>
        /// Gets the coach id for the Application User Id passed in
        /// </summary>
        /// If there is an Coach with that Application User Id it sets coachId to  
        /// the value of Coach.Id, and returns true.
        /// If there is not an Coach with that Application User Id, coachId is 0,
        /// and the method returns false.
        /// <param name="applicationUserId"></param>
        /// <param name="coachId"></param>
        /// <returns>
        /// True if the given coachId exists in the context, and coachId is set accordingly
        /// </returns>
        public bool GetCoachId(string applicationUserId, out int coachId);

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
        /// <returns>
        /// An AddAthleteToCoachResponseDTO with the result of the action.
        /// </returns>
        public Task<AddAthleteToCoachResponseDTO> AddAthleteToCoach(int coachId, int athleteId);

        /// <summary>
        /// Gets the Coach details associated with the given coachId.
        /// </summary>
        /// If the coachId is not associated with a Coach record, a CoachDetailsResponseDTO is returned
        /// with Success set to false, and an error message in the Error dictionary. 
        /// 
        /// If the coachId is associated with a Coach record, a CoachDetailsResponseDTO is returned with 
        /// details of the Coach record, and Success is set to true.
        /// <param name="coachId"></param>
        /// <returns>
        /// A CoachDetailsResponseDTO with the result of the action.
        /// </returns>
        public Task<CoachDetailsResponseDTO> Details(int coachId);

        /// <summary>
        /// Updates the details of a Coach record.
        /// </summary>
        /// If the coachId is not associated with a Coach record, a CoachDetailsResponseDTO is returned
        /// with Success set to false, and an error message in the Error dictionary.
        /// 
        /// If the coachId is associated with a Coach record, the record is updated with the details given
        /// and a CoachDetailsResponseDTO is returned with details of the Coach record, and Success is set to true.
        /// <param name="coachId"></param>
        /// <param name="editDetailsDTO"></param>
        /// <returns>
        /// A CoachDetailsResponseDTO with the result of the action.
        /// </returns>
        public Task<CoachDetailsResponseDTO> EditDetails(int coachId, EditDetailsDTO editDetailsDTO);

        /// <summary>
        /// Deletes an Coach from the database.
        /// </summary>
        /// Returns a DeleteCoachDTO with Success flag set to false if the given coachId is not
        /// associated with an Coach in the db.
        /// Otherwise it will have Success set to true.
        /// <param name="coachId"></param>
        /// <returns>
        /// A DeleteCoachDTO with the result of the action.
        /// </returns>
        public Task<DeleteCoachDTO> Delete(int coachId);
    }
}
