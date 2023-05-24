using Microsoft.AspNetCore.Identity;

using API.Data.Models;
using API.JWT;
using API.Data.Managers.Interfaces;

using DTO.Account;

namespace API.Data.Managers
{
    /// <summary>
    /// Manages the creation and updating of user accounts.
    /// </summary>
    /// The application uses two data contexts to maintain user information. A UserContext which extends IdentityDbContext and uses
    /// that to manage the user functionality (like managing roles, checking emails/passwords etc). And a WeightliftingContext which
    /// handles all the weightlifting specific data, like if they're a coach/athlete, their sessions, their exercises etc. 
    /// 
    /// The AccountManager brings these together. So when a new User is created, an associated Athlete or Coach is created and they're 
    /// tied together using the ApplicationUserId.
    /// 
    /// The AccountManger also sends a JWToken back to a logged in User so they can access authorized routes.
    /// 
    public class AccountManager : IAccountManager
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJWTHandler _jwtHandler;
        private readonly WeightliftingContext _weightliftingContext;

        public AccountManager(UserManager<ApplicationUser> userManager, IJWTHandler jwtHandler, WeightliftingContext weightliftingContext)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _weightliftingContext = weightliftingContext;
        }

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
        public async Task<UserRegistrationResponseDTO> RegisterAthlete(UserRegistrationDTO userRegistrationDTO)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = userRegistrationDTO.Email,
                FirstName = userRegistrationDTO.FirstName,
                Email = userRegistrationDTO.Email
            };

            var result = await _userManager.CreateAsync(user, userRegistrationDTO.Password);

            if (!result.Succeeded)
            {
                return new UserRegistrationResponseDTO()
                {
                    Success = false,
                    Errors = new List<string> { "Error registering user" }
                };
            }

            // add Athlete role
            await _userManager.AddToRoleAsync(user, UserRoles.Athlete);

            // add new Athlete object to db
            await _weightliftingContext.Athletes.AddAsync(new Athlete()
            {
                FirstName = userRegistrationDTO.FirstName,
                ApplicationUserId = user.Id
            });
            _weightliftingContext.SaveChanges();

            return new UserRegistrationResponseDTO
            {
                Success = true
            };
        }

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
        public async Task<UserRegistrationResponseDTO> RegisterCoach(UserRegistrationDTO userRegistrationDTO)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = userRegistrationDTO.Email,
                FirstName = userRegistrationDTO.FirstName,
                Email = userRegistrationDTO.Email
            };

            var result = await _userManager.CreateAsync(user, userRegistrationDTO.Password);

            if (!result.Succeeded)
            {
                return new UserRegistrationResponseDTO()
                {
                    Success = false,
                    Errors = new List<string> { "Error registering user" }
                };
            }

            // add Coach role
            await _userManager.AddToRoleAsync(user, UserRoles.Coach);

            // add new Coach object to db
            await _weightliftingContext.Coaches.AddAsync(new Coach()
            {
                FirstName = userRegistrationDTO.FirstName,
                ApplicationUserId = user.Id
            });
            _weightliftingContext.SaveChanges();

            return new UserRegistrationResponseDTO
            {
                Success = true
            };
        }

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
        public async Task<UserAuthenticationResponseDTO> Login(UserAuthenticationDTO userAuthenticationDTO)
        {
            // Check user exists
            var user = await _userManager.FindByEmailAsync(userAuthenticationDTO.Email);

            if (user is null)
            {
                return new UserAuthenticationResponseDTO()
                {
                    Success = false,
                    Errors = new List<string> { "Email not found." }
                };
            }

            // Check password correct
            var passwordCheck = await _userManager.CheckPasswordAsync(user, userAuthenticationDTO.Password);

            if (!passwordCheck)
            {
                return new UserAuthenticationResponseDTO()
                {
                    Success = false,
                    Errors = new List<string> { "Password incorrect." }
                };
            }

            // Get JWT for authorised routes
            var token = await _jwtHandler.GetToken(user);

            return new UserAuthenticationResponseDTO()
            {
                Success = true,
                Token = token
            };
        }

        /// <summary>
        /// Deletes the Account, including related Coach or Athlete objects.
        /// </summary>
        /// Attempts to delete the user associated with the given userEmail.
        /// If the user's email is not found, returns a UserAuthenticationResponseDTO with Success set to false, 
        /// and an error message in the Errors dictionary.
        /// If the user is successfully deleted, associated Coach or Athlete instances are found and removed. 
        /// It then returns a DeleteAccountDTO with Success set to true.
        /// <param name="userEmail"></param>
        /// <returns>
        /// A DeleteAccountDTO with the result of the action.
        /// </returns>
        public async Task<DeleteAccountDTO> Delete(string userEmail)
        {
            // Check user exists
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user is null)
            {
                return new DeleteAccountDTO()
                {
                    Success = false,
                    Errors = new List<string> { "Email not found." }
                };
            }

            // delete user
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return new DeleteAccountDTO()
                {
                    Success = false,
                    Errors = new List<string> { "Error in UserManager deleting user." }
                };
            }

            // delete coach/athlete if they exist
            var coach = _weightliftingContext.Coaches.FirstOrDefault(coach => coach.ApplicationUserId == user.Id);

            if (coach is not null)
            {
                _weightliftingContext.Coaches.Remove(coach);
            }

            var athlete = _weightliftingContext.Athletes.FirstOrDefault(athlete => athlete.ApplicationUserId == user.Id);

            if (athlete is not null)
            {
                _weightliftingContext.Athletes.Remove(athlete);
            }

            // save changes
            _weightliftingContext.SaveChanges();

            // return 
            return new DeleteAccountDTO()
            {
                Success = true
            };
        }
    }
}
