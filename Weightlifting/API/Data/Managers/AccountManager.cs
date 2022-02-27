using Microsoft.AspNetCore.Identity;

using AutoMapper;

using API.Data.Models;
using API.DTOs.Account;
using API.JWT;


namespace API.Data.Managers
{
    public class AccountManager : IAccountManager
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTHandler _jwtHandler;
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;

        public AccountManager(UserManager<ApplicationUser> userManager, JWTHandler jwtHandler, IMapper mapper, DatabaseContext context)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
            _context = context;
        }

        public Task<UserRegistrationResponseDTO> Register(UserRegistrationDTO userRegistrationDTO)
        {
            throw new NotImplementedException();
        }

        public Task<UserAuthenticationResponseDTO> Login(UserAuthenticationDTO userAuthenticationDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}
