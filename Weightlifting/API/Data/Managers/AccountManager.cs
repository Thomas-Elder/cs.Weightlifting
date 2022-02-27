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
        private readonly UserContext _context;

        public AccountManager(UserManager<ApplicationUser> userManager, IJWTHandler jwtHandler, UserContext context)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _context = context;
        }

        public async Task<UserRegistrationResponseDTO> Register(UserRegistrationDTO userRegistrationDTO)
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
                    isSuccessfulRegistration = false,
                    Errors = new Dictionary<string, string> {
                        {
                        "Registration", "Error registering user"
                        }
                    }
                };
            }

            return new UserRegistrationResponseDTO
            {
                isSuccessfulRegistration = true
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
                    IsSuccess = false,
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
                    IsSuccess = false,
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
                IsSuccess = true,
                Token = token
            };
        }
    }
}
