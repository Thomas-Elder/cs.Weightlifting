using Xunit;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;

using API.Data.Managers;
using API.Data.Models;
using API.DTOs.Account;
using API.Data;
using API.JWT;
using System.Threading.Tasks;

namespace API.Tests.Data.Managers
{
    public class AccountManagerTests: IDisposable
    {
        private AccountManager _sut;
        private UserManager<ApplicationUser> mock_UserManager;
        private DatabaseContext mock_DatabaseContext;
        private IJWTHandler mock_JWTHandler;

        public AccountManagerTests()
        {
            mock_UserManager = Substitute.For<UserManager<ApplicationUser>>(
                Substitute.For<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null
                );

            mock_JWTHandler = Substitute.For<IJWTHandler>();

            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "WeightliftingTest")
                .Options;

            mock_DatabaseContext = new DatabaseContext(options);

            _sut = new AccountManager(
                mock_UserManager,
                mock_JWTHandler,
                mock_DatabaseContext
                );
        }

        public void Dispose()
        {
            mock_DatabaseContext.Database.EnsureDeleted();
        }

        #region Login
        [Fact]
        public async void Login_WhenCalledWithNonExistentUserDetails_ReturnsUserAuthenticationResponseDTOWithIsSuccessFalse()
        {
            // Arrange
            UserAuthenticationDTO userAuthenticationDTO = new UserAuthenticationDTO()
            {
                UserName = "Non existent user",
                Password = "Any"
            };

            mock_UserManager.FindByEmailAsync(Arg.Any<string>()).ReturnsNull();

            // Act
            var result = await _sut.Login(userAuthenticationDTO);

            // Assert
            Assert.False(result.IsSuccess);
        }
        
        [Fact]
        public async void Login_WhenCalledWithExistingUserButIncorrectPassword_ReturnsUserAuthenticationResponseDTOWithIsSuccessFalse()
        {
            // Arrange
            UserAuthenticationDTO userAuthenticationDTO = new UserAuthenticationDTO()
            {
                UserName = "Existing user",
                Password = "Wrong"
            };

            mock_UserManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult(Substitute.For<ApplicationUser>()));

            mock_UserManager.CheckPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(Task.FromResult(false));

            // Act
            var result = await _sut.Login(userAuthenticationDTO);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async void Login_WhenCalledWithExistingUserAnCorrectPassword_ReturnsUserAuthenticationResponseDTOWithIsSuccessTrue()
        {
            // Arrange
            UserAuthenticationDTO userAuthenticationDTO = new UserAuthenticationDTO()
            {
                UserName = "Existing user",
                Password = "Correct"
            };

            mock_UserManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult(Substitute.For<ApplicationUser>()));

            mock_UserManager.CheckPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(Task.FromResult(true));

            // Act
            var result = await _sut.Login(userAuthenticationDTO);

            // Assert
            Assert.True(result.IsSuccess);
        }
        #endregion

        #region Register
        [Fact]
        public async void Register_WhenCalledWithExistingEmail_ReturnsUserRegistrationResponseDTOWithisSuccessfulRegistrationFalse()
        {
            // Arrange
            UserRegistrationDTO userRegistrationDTO = new UserRegistrationDTO() 
            { 
                FirstName = "New",
                Email = "existing@gmail.com",
                Password = "match",
                ConfirmPassword = "match"
            };
            
            mock_UserManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Failed());

            // Act
            var result = await _sut.Register(userRegistrationDTO);

            // Assert
            Assert.False(result.isSuccessfulRegistration);
        }

        [Fact]
        public async void Register_WhenCalledWithUniqueEmail_ReturnsUserRegistrationResponseDTOWithisSuccessfulRegistrationTrue()
        {
            // Arrange
            UserRegistrationDTO userRegistrationDTO = new UserRegistrationDTO()
            {
                FirstName = "New",
                Email = "unique@gmail.com",
                Password = "match",
                ConfirmPassword = "match"
            };

            mock_UserManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            // Act
            var result = await _sut.Register(userRegistrationDTO);

            // Assert
            Assert.True(result.isSuccessfulRegistration);
        }
        #endregion
    }
}
