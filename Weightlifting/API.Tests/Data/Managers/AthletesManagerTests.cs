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

            mock_WeightliftingContext.Athletes.Add(new Athlete()
            {
                Id = 2,
                FirstName = "Test",
                LastName = "Athlete",
                ApplicationUserId = "2"
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
            string nonExistentApplicationUserId = "5";

            // Act
            var result = _sut.GetAthleteId(nonExistentApplicationUserId, out int athleteId);

            // Assert
            Assert.False(result);
            Assert.Equal(0, athleteId);
        }

        [Fact]
        public void GetAthleteId_WhenCalledWithExistentUserID_ReturnsTrueAndSetsAthleteIdParameter()
        {
            // Arrange
            string existentApplicationUserId = "2";

            // Act
            var result = _sut.GetAthleteId(existentApplicationUserId, out int athleteId);

            // Assert
            Assert.True(result);
            Assert.Equal(1, athleteId);
        }
        #endregion

        #region AddAthleteToCoach
        [Fact]
        public async void AddCoach_WhenCalledWithNonExistentAthleteID_ReturnsAddAthleteToCoachResponseDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.AddCoach(3, 1);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddCoach_WhenCalledWithNonExistentCoachID_ReturnsAddAthleteToCoachResponseDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.AddCoach(1, 2);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddCoach_WhenCalledWithExistingIDs_ReturnsAddAthleteToCoachResponseDTOWithSuccessTrue()
        {
            // Arrange

            // Act
            var result = await _sut.AddCoach(2, 1);

            // Assert
            Assert.True(result.Success);
        }
        #endregion

        #region Details
        [Fact]
        public async void Details_WhenCalledWithNonExistentAthleteId_ReturnsAthleteDetailsDTOWithSuccessFalse()
        {
            // Arrange
            int nonExistentAthleteId = 5;

            // Act
            var result = await _sut.Details(nonExistentAthleteId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void Details_WhenCalledWithExistentAthleteId_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange
            int existentAthleteId = 1;
            var expectedAthlete = new 
            {
                FirstName = "Test",
                LastName = "Athlete"
            };
            var expectedCoach = new
            {
                FirstName = "Test",
                LastName = "Coach"
            };

            // Act
            var result = await _sut.Details(existentAthleteId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(expectedAthlete.FirstName, result.FirstName);
            Assert.Equal(expectedAthlete.LastName, result.LastName);

            Assert.Equal(1, result.Coach.CoachId);
            Assert.Equal(expectedCoach.FirstName, result.Coach.FirstName);
            Assert.Equal(expectedCoach.LastName, result.Coach.LastName);

            Assert.Single(result.Sessions);
        }

        [Fact]
        public async void Details_WhenCalledWithExistentAthleteIdWithNoCoach_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange
            int existentAthleteId = 2;
            var expectedAthlete = new
            {
                FirstName = "Test",
                LastName = "Athlete"
            };

            // Act
            var result = await _sut.Details(existentAthleteId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(expectedAthlete.FirstName, result.FirstName);
            Assert.Equal(expectedAthlete.LastName, result.LastName);

            Assert.Null(result.Coach);
        }
        #endregion

        #region EditDetails
        [Fact]
        public async void EditDetails_WhenCalledWithNonExistentAthleteId_ReturnsAthleteDetailsDTOWithSuccessFalse()
        {
            // Arrange
            int nonExistentAthleteId = 5;
            EditDetailsDTO editDetailsDTO = new EditDetailsDTO()
            {
                LastName = "Updated last name"
            };

            // Act
            var result = await _sut.EditDetails(nonExistentAthleteId, editDetailsDTO);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void EditDetails_WhenCalledWithExistentAthleteId_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange
            int existentAthleteId = 1;
            EditDetailsDTO editDetailsDTO = new EditDetailsDTO()
            {
                FirstName = "Updated first name",
                LastName = "Updated last name"
            };
            var expectedAthlete = new
            {
                FirstName = "Updated first name",
                LastName = "Updated last name"
            };

            // Act
            var result = await _sut.EditDetails(existentAthleteId, editDetailsDTO);
            var updatedAthlete = await mock_WeightliftingContext.Athletes.FirstOrDefaultAsync(a => a.Id == 1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(expectedAthlete.FirstName, result.FirstName);
            Assert.Equal(expectedAthlete.LastName, result.LastName);
        }
        #endregion
    }
}
