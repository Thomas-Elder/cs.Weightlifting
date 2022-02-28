using Microsoft.AspNetCore.Identity;

using API.Data.Models;
using API.DTOs.Account;
using API.JWT;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Managers
{
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
                    Errors = new Dictionary<string, string> {
                        {
                        "Registration", "Error registering user"
                        }
                    }
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
                    Errors = new Dictionary<string, string> {
                        {
                        "Registration", "Error registering user"
                        }
                    }
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

        public async Task<UserAuthenticationResponseDTO> Login(UserAuthenticationDTO userAuthenticationDTO)
        {
            // Check user exists
            var user = await _userManager.FindByEmailAsync(userAuthenticationDTO.UserName);

            if (user == null)
            {
                return new UserAuthenticationResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>
                    {
                        {
                            "Email",  "Email not found."
                        }
                    }
                };
            }

            // Check password correct
            var passwordCheck = await _userManager.CheckPasswordAsync(user, userAuthenticationDTO.Password);

            if (!passwordCheck)
            {
                return new UserAuthenticationResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>
                    {
                        {
                            "Password",  "Password incorrect."
                        }
                    }
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
    }
}
