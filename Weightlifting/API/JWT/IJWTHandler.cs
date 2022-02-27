using API.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.JWT
{
    public interface IJWTHandler
    {
        public Task<string> GetToken(ApplicationUser user);
        public SigningCredentials GetSigningCredentials();
        public Task<List<Claim>> GetClaims(ApplicationUser user);
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
    }
}
