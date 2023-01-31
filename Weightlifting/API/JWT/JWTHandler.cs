using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using API.Data.Models;

namespace API.JWT
{
    public class JWTHandler : IJWTHandler
    {
        private readonly IConfigurationSection _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public JWTHandler(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _jwtSettings = configuration.GetSection("JWTSettings");
            _userManager = userManager;
        }

        /// <summary>
        /// Returns a string, a serialized JWTSecurityToken (JSON Web Signature in 'Compact Serialization Format'), used
        /// by the consumer of the API to authorise their access to specified endpoints. 
        /// </summary>
        /// This process has been broken into a few steps. 
        /// 1 - Create SigningCredentials based on our secret in appsettings.json
        /// 2 - Get user claims from the userManager, this is where the user role is registered, so this allows
        ///     Coaches access to Coach endpoints for example. 
        /// 3 - Generate a JWTSecurityToken using the SigningCredentials and user Claims
        /// 4 - This is then serialised for returning to the consumer of the API.
        /// 
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> GetToken(ApplicationUser user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }

        /// <summary>
        /// Generates the SigningCredentials, using the secret key stored in appsettings.json
        /// </summary>
        /// <returns>
        /// Returns a new SigningCredentials
        /// </returns>
        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("Key").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// Asynchronously gets a list of Claims for this user from the UserManager.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// Returns a task, which resolves to a list of Claims.
        /// </returns>
        public async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        /// <summary>
        /// Creates the JWTSecurity token using the signingCredentials and claims passed. 
        /// </summary>
        /// <param name="signingCredentials"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.GetSection("Issuer").Value,
                audience: _jwtSettings.GetSection("Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.GetSection("Expiry").Value)),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }
    }
}
