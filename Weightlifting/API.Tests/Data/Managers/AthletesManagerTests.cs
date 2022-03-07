using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

using API.Data;
using API.Data.Models;
using API.Data.Managers;
using API.DTOs.Athletes;

namespace API.Tests.Data.Managers
{
    public class AthletesManagerTests : IDisposable
    {
        AthletesManager _sut;

        WeightliftingContext mock_WeightliftingContext;


        public AthletesManagerTests()
        {
            var weightliftingOptions = new DbContextOptionsBuilder<WeightliftingContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            mock_WeightliftingContext = new WeightliftingContext(weightliftingOptions);

            mock_WeightliftingContext.Coaches.Add(new Coach()
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Coach",
                ApplicationUserId = "1",
            });

            mock_WeightliftingContext.Athletes.Add(new Athlete()
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Athlete",
                ApplicationUserId = "2",

            });

            mock_WeightliftingContext.SaveChanges();

            _sut = new AthletesManager(mock_WeightliftingContext);
        }

        public void Dispose()
        {
            mock_WeightliftingContext.Database.EnsureDeleted();
        }

        #region AddAthleteToCoach
        [Fact]
        public async void AddCoach_WhenCalledWithNonExistentUserID_ReturnsAddAthleteToCoachResponseDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.AddCoach("1", 1);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddCoach_WhenCalledWithNonExistentCoachID_ReturnsAddAthleteToCoachResponseDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.AddCoach("1", 2);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddCoach_WhenCalledWithExistingIDs_ReturnsAddAthleteToCoachResponseDTOWithSuccessTrue()
        {
            // Arrange

            // Act
            var result = await _sut.AddCoach("2", 1);

            // Assert
            Assert.True(result.Success);
        }
        #endregion

        #region AddSession
        [Fact]
        public async void AddSession_WhenCalledWithNonExistentAthleteApplicationUserId_ReturnsAddSessionResponseDTOWithSuccessFalse()
        {
            // Arrange
            var addSessionDTO = new AddSessionDTO()
            {
                Date = new DateTime(2022, 1, 1)
            };

            // Act
            var result = await _sut.AddSession("1", addSessionDTO);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddSession_WhenCalledWithExistentAthleteApplicationUserId_ReturnsAddSessionResponseDTOWithSuccessTrue()
        {
            // Arrange
            var addSessionDTO = new AddSessionDTO()
            {
                Date = new DateTime(2022, 1, 1)
            };

            // Act
            var result = await _sut.AddSession("2", addSessionDTO);

            // Assert
            Assert.True(result.Success);
        }
        #endregion

        #region AthleteDetails
        [Fact]
        public async void AthleteDetails_WhenCalledWithNonExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.AthleteDetails("1");

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AthleteDetails_WhenCalledWithExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange

            // Act
            var result = await _sut.AthleteDetails("2");

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Athlete", result.LastName);
        }
        #endregion
    }
}
