using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

using API.Data;
using API.Data.Models;
using API.Data.Managers;
using API.Data.Managers.Interfaces;
using API.DTOs.Athletes;

namespace API.Tests.Data.Managers
{
    public class AthletesManagerTests : IDisposable
    {
        IAthletesManager _sut;

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
                CoachId = 1
            });

            mock_WeightliftingContext.Sessions.Add(new Session()
            {
                Id = 1,
                Date = new DateTime(2022, 1, 1),
                AthleteId = 1
            });

            mock_WeightliftingContext.SaveChanges();

            _sut = new AthletesManager(mock_WeightliftingContext);
        }

        public void Dispose()
        {
            mock_WeightliftingContext.Database.EnsureDeleted();
        }

        #region GetAthleteId
        [Fact]
        public void GetAthleteId_WhenCalledWithNonExistentUserID_ReturnsFalse()
        {
            // Arrange

            // Act
            var result = _sut.GetAthleteId("1", out int athleteId);

            // Assert
            Assert.False(result);
            Assert.Equal(0, athleteId);
        }

        [Fact]
        public void GetAthleteId_WhenCalledWithExistentUserID_ReturnsTrueAndSetsAthleteIdParameter()
        {
            // Arrange

            // Act
            var result = _sut.GetAthleteId("2", out int athleteId);

            // Assert
            Assert.True(result);
            Assert.Equal(1, athleteId);
        }
        #endregion

        #region UserIsAthlete
        [Fact]
        public void UserIsAthlete_WhenCalledWithUserIDNotAnAthlete_ReturnsFalse()
        {
            // Arrange

            // Act
            var result = _sut.UserIsAthlete("1", 1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UserIsAthlete_WhenCalledWithUserIDIsAnAthlete_ReturnsTrue()
        {
            // Arrange

            // Act
            var result = _sut.UserIsAthlete("2", 1);

            // Assert
            Assert.True(result);
        }
        #endregion

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

        #region Details
        [Fact]
        public async void Details_WhenCalledWithNonExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.Details(2);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void Details_WhenCalledWithExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange

            // Act
            var result = await _sut.Details(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Athlete", result.LastName);

            Assert.Equal(1, result.Coach.CoachId);
            Assert.Equal("Test", result.Coach.FirstName);
            Assert.Equal("Coach", result.Coach.LastName);

            Assert.Single(result.Sessions);
        }
        #endregion

        #region EditDetails
        [Fact]
        public async void EditDetails_WhenCalledWithNonExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessFalse()
        {
            // Arrange
            EditDetailsDTO editDetailsDTO = new EditDetailsDTO()
            {
                LastName = "Updated last name"
            };

            // Act
            var result = await _sut.EditDetails(2, editDetailsDTO);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void EditDetails_WhenCalledWithExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange
            EditDetailsDTO editDetailsDTO = new EditDetailsDTO()
            {
                LastName = "Updated last name"
            };

            // Act
            var result = await _sut.EditDetails(1, editDetailsDTO);
            var updatedAthlete = await mock_WeightliftingContext.Athletes.FirstOrDefaultAsync(a => a.Id == 1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Updated last name", updatedAthlete.LastName);
            Assert.Equal("Test", updatedAthlete.FirstName);
        }
        #endregion
    }
}
