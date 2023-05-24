using DTO.Sessions;

namespace API.Data.Managers.Interfaces
{
    public interface ISessionsManager
    {
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
        public Task<AddSessionResponseDTO> AddSession(AddSessionDTO addSessionDTO);

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
        public Task<SessionDetailsDTO> Details(int sessionId);

        /// <summary>
        /// Updates the details of a Session.
        /// </summary>
        /// Checks if the sessionId matches an existing Session in the database, if not, returns a SessionDetailsDTO
        /// with Success set to false, and Error message in the Errors dict.
        /// If the Session exists, the details of the Session are updated, and the updated details are returned in a 
        /// editSessionDetailsDTO, with Success set to true.
        /// <param name="editSessionDetailsDTO"></param>
        /// <returns>
        /// A editSessionDetailsDTO with the result of the action. 
        /// </returns>
        public Task<EditSessionDetailsResponseDTO> EditDetails(EditSessionDetailsDTO editSessionDetailsDTO);

        /// <summary>
        /// Deletes a Session.
        /// </summary>
        /// Checks if the sessionId matches an existing Session in the database, if not, returns a DeleteSessionDTO
        /// with Success set to false, and Error message in the Errors dict.
        /// If the session exists, it is deleted from the database.
        /// <param name="sessionId"></param>
        /// <returns>
        /// A DeleteSessionDTO with the result of the action. 
        /// </returns>
        public Task<DeleteSessionDTO> Delete(int sessionId);
    }
}
