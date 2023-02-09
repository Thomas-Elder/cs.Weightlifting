using API.Data.Models;
using API.DTOs.Account;

namespace API.Data.Managers.Interfaces
{
    public interface IAccountManager
    {
        /// <summary>
        /// Registers a new Athlete. 
        /// </summary>
        /// Creates a new ApplicationUser, and a new Athlete. The new ApplicationUser is assigned the Athlete role.
        /// The new Athlete has ApplicationUserId set to the ApplicationUser id.
        /// If the creation of a new ApplicationUser fails, a UserRegistrationResponseDTO is returned with Success
        /// set to false, and an error message in the Errors dictionary.
        /// <param name="userRegistrationDTO"></param>
        /// <returns>
        /// A UserRegistrationResponseDTO with the result of the action.
        /// </returns>
        public Task<UserRegistrationResponseDTO> RegisterCoach(UserRegistrationDTO userRegistrationDTO);

        /// <summary>
        /// Registers a new Athlete. 
        /// </summary>
        /// Creates a new ApplicationUser, and a new Coach. The new ApplicationUser is assigned the Coach role.
        /// The new Coach has ApplicationUserId set to the ApplicationUser id.
        /// If the creation of a new ApplicationUser fails, a UserRegistrationResponseDTO is returned with Success
        /// set to false, and an error message in the Errors dictionary.
        /// <param name="userRegistrationDTO"></param>
        /// <returns>
        /// A UserRegistrationResponseDTO with the result of the action.
        /// </returns>
        public Task<UserRegistrationResponseDTO> RegisterAthlete(UserRegistrationDTO userRegistrationDTO);

        /// <summary>
        /// Logs the user in.
        /// </summary>
        /// Attempts to log a user in to the application.
        /// If the user's email is not found, returns a UserAuthenticationResponseDTO with Success set to false, 
        /// and an error message in the Errors dictionary.
        /// If the user's password is incorrect, returns a UserAuthenticationResponseDTO with Success set to false, 
        /// and an error message in the Errors dictionary.
        /// If the user is successfully logged in, a JWToken is created to represent the user's permissions and it
        /// is added to the UserAuthenticationResponseDTO.
        /// <param name="userAuthenticationDTO"></param>
        /// <returns>
        /// A UserAuthenticationResponseDTO with the result of the action.
        /// </returns>
        public Task<UserAuthenticationResponseDTO> Login(UserAuthenticationDTO userAuthenticationDTO);

        /// <summary>
        /// Deletes the Account, including related Coach or Athlete objects.
        /// </summary>
        /// Attempts to delete the user associated with the given userEmail.
        /// If the user's email is not found, returns a UserAuthenticationResponseDTO with Success set to false, 
        /// and an error message in the Errors dictionary.
        /// If the user is successfully deleted, returns a DeleteAccountDTO with Success set to true.
        /// <param name="userEmail"></param>
        /// <returns>
        /// A DeleteAccountDTO with the result of the action.
        /// </returns>
        public Task<DeleteAccountDTO> Delete(string userEmail);
    }
}
