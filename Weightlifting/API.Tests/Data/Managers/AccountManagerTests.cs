using Xunit;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;

using API.Data.Managers;
using API.Data.Managers.Interfaces;
using API.Data.Models;
using API.DTOs.Account;
using API.Data;
using API.JWT;
using System.Threading.Tasks;

namespace API.Tests.Data.Managers
{
    public class AccountManagerTests: IDisposable
    {
        UserManager<ApplicationUser> mock_UserManager;
        IJWTHandler mock_JWTHandler;
        WeightliftingContext mock_WeightliftingContext;
        AccountManager _sut;

        public AccountManagerTests()
        {
            mock_UserManager = Substitute.For<UserManager<ApplicationUser>>(
                Substitute.For<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null
                );

            mock_JWTHandler = Substitute.For<IJWTHandler>();

            var weightliftingOptions = new DbContextOptionsBuilder<WeightliftingContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            mock_WeightliftingContext = new WeightliftingContext(weightliftingOptions);

            _sut = new AccountManager(
                mock_UserManager,
                mock_JWTHandler,
                mock_WeightliftingContext
                );
        }

        public void Dispose()
        {
            mock_WeightliftingContext.Database.EnsureDeleted();
        }

        #region Login
        [Fact]
        public async void Login_WhenCalledWithNonExistentUserDetails_ReturnsUserAuthenticationResponseDTOWithIsSuccessFalse()
        {
            // Arrange
            UserAuthenticationDTO userAuthenticationDTO = new UserAuthenticationDTO()
            {
                Email = "Non existent user",
                Password = "Any"
            };

            mock_UserManager.FindByEmailAsync(Arg.Any<string>()).ReturnsNull();

            // Act
            var result = await _sut.Login(userAuthenticationDTO);

            // Assert
            Assert.False(result.Success);
        }
        
        [Fact]
        public async void Login_WhenCalledWithExistingUserButIncorrectPassword_ReturnsUserAuthenticationResponseDTOWithIsSuccessFalse()
        {
            // Arrange
            UserAuthenticationDTO userAuthenticationDTO = new UserAuthenticationDTO()
            {
                Email = "Existing user",
                Password = "Wrong"
            };

            mock_UserManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult(Substitute.For<ApplicationUser>()));

            mock_UserManager.CheckPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(Task.FromResult(false));

            // Act
            var result = await _sut.Login(userAuthenticationDTO);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void Login_WhenCalledWithExistingUserAnCorrectPassword_ReturnsUserAuthenticationResponseDTOWithIsSuccessTrue()
        {
            // Arrange
            UserAuthenticationDTO userAuthenticationDTO = new UserAuthenticationDTO()
            {
                Email = "Existing user",
                Password = "Correct"
            };

            mock_UserManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult(Substitute.For<ApplicationUser>()));

            mock_UserManager.CheckPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(Task.FromResult(true));

            // Act
            var result = await _sut.Login(userAuthenticationDTO);

            // Assert
            Assert.True(result.Success);
        }
        #endregion

        #region RegisterAthlete
        [Fact]
        public async void RegisterAthlete_WhenCalledWithExistingEmail_ReturnsUserRegistrationResponseDTOWithisSuccessfulRegistrationFalse()
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
            var result = await _sut.RegisterAthlete(userRegistrationDTO);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void RegisterAthlete_WhenCalledWithUniqueEmail_ReturnsUserRegistrationResponseDTOWithisSuccessfulRegistrationTrue()
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
            var result = await _sut.RegisterAthlete(userRegistrationDTO);

            // Assert
            Assert.True(result.Success);
        }
        #endregion

        #region RegisterCoach
        [Fact]
        public async void RegisterCoach_WhenCalledWithExistingEmail_ReturnsUserRegistrationResponseDTOWithisSuccessfulRegistrationFalse()
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
            var result = await _sut.RegisterCoach(userRegistrationDTO);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void RegisterCoach_WhenCalledWithUniqueEmail_ReturnsUserRegistrationResponseDTOWithisSuccessfulRegistrationTrue()
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
            var result = await _sut.RegisterCoach(userRegistrationDTO);

            // Assert
            Assert.True(result.Success);
        }
        #endregion

        #region Delete
        [Fact]
        public async void Delete_WhenCalledWithNonExistentUserDetails_ReturnsDeleteAccountDTOWithIsSuccessFalse()
        {
            // Arrange
            string email = "Non existing user";

            mock_UserManager.FindByEmailAsync(Arg.Any<string>()).ReturnsNull();

            // Act
            var result = await _sut.Delete(email);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void Delete_WhenCalledWithExistentUserDetailsAndDeletionFails_ReturnsDeleteAccountDTOWithIsSuccessFalse()
        {
            // Arrange
            string email = "Non existing user";

            mock_UserManager.FindByEmailAsync(Arg.Any<string>())
                .Returns(Task.FromResult(Substitute.For<ApplicationUser>()));

            mock_UserManager.DeleteAsync(Arg.Any<ApplicationUser>())
                .Returns(Task.FromResult(IdentityResult.Failed(
                    new IdentityError() { Code= "Removal error", Description = "Error in UserManager deleting user." })));

            // Act
            var result = await _sut.Delete(email);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void Delete_WhenCalledWithExistentUserDetailsAndDeletionIsSuccessful_ReturnsDeleteAccountDTOWithIsSuccessTrue()
        {
            // Arrange
            string email = "Non existing user";

            mock_UserManager.FindByEmailAsync(Arg.Any<string>())
                .Returns(Task.FromResult(Substitute.For<ApplicationUser>()));

            mock_UserManager.DeleteAsync(Arg.Any<ApplicationUser>())
                .Returns(Task.FromResult(IdentityResult.Success));

            // Act
            var result = await _sut.Delete(email);

            // Assert
            Assert.True(result.Success);
        }

        // Check account deleted 

        // Check athlete removed

        // Check coach removed

        #endregion
    }
}
