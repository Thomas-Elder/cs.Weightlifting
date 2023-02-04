using API.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.JWT
{
    public interface IJWTHandler
    {
        /// <summary>
        /// Returns a string, a serialized JWTSecurityToken (JSON Web Signature in 'Compact Serialization Format'), used
        /// by the consumer of the API to authorise their access to specified endpoints. 
        /// </summary>
        /// Takes an ApplicationUser, creates SigningCredentials, gets the user's claims, generates the token options, 
        /// then creates a new JWT for the user to include in requests to the API for authorisation. 
        /// <param name="user"></param>
        /// <returns>
        /// A JSON Web Token which includes serialised information about the user's permissions.
        /// </returns>
        public Task<string> GetToken(ApplicationUser user);

        /// <summary>
        /// Generates the SigningCredentials, using the secret key stored in appsettings.json
        /// </summary>
        /// <returns>
        /// Returns a new SigningCredentials
        /// </returns>
        public SigningCredentials GetSigningCredentials();

        /// <summary>
        /// Asynchronously gets a list of Claims for this user from the UserManager.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// Returns a task, which resolves to a list of Claims.
        /// </returns>
        public Task<List<Claim>> GetClaims(ApplicationUser user);

        /// <summary>
        /// Creates the JWTSecurity token using the signingCredentials and claims passed. 
        /// </summary>
        /// <param name="signingCredentials"></param>
        /// <param name="claims"></param>
        /// <returns>
        /// Returns a JwtSecurityToken.
        /// </returns>
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
    }
}
