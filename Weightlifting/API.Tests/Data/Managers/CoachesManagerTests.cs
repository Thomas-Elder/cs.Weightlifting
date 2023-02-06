using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

using API.Data;
using API.Data.Models;
using API.Data.Managers;
using API.Data.Managers.Interfaces;
using API.DTOs.Coaches;

namespace API.Tests.Data.Managers
{
    public class CoachesManagerTests : IDisposable
    {
        private readonly CoachesManager _sut;

        private readonly WeightliftingContext mock_WeightliftingContext;

        public CoachesManagerTests()
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
                ApplicationUserId = "2",
            });

            mock_WeightliftingContext.SaveChanges();

            _sut = new CoachesManager(mock_WeightliftingContext);
        }

        public void Dispose()
        {
            mock_WeightliftingContext.Database.EnsureDeleted();
        }

        #region GetCoachId
        [Fact]
        public void GetCoachId_WhenCalledWithNonExistentUserID_ReturnsFalse()
        {
            // Arrange
            string nonExistentUserId = "5";
            int expectedCoachId = 0;

            // Act
            var result = _sut.GetCoachId(nonExistentUserId, out int coachId);

            // Assert
            Assert.False(result);
            Assert.Equal(expectedCoachId, coachId);
        }

        [Fact]
        public void GetCoachId_WhenCalledWithExistentUserID_ReturnsTrueAndSetsAthleteIdParameter()
        {
            // Arrange
            string existentUserId = "1";
            int expectedCoachId = 1;

            // Act
            var result = _sut.GetCoachId(existentUserId, out int coachId);

            // Assert
            Assert.True(result);
            Assert.Equal(expectedCoachId, coachId);
        }
        #endregion

        #region AddAthleteToCoach
        [Fact]
        public async void AddAthleteToCoach_WhenCalledWithNonExistentCoachID_ReturnsAddAthleteToCoachResponseDTOWithSuccessFalse()
        {
            // Arrange
            int nonExistentCoachId = 5;
            int existentAthleteId = 1;

            // Act
            var result = await _sut.AddAthleteToCoach(nonExistentCoachId, existentAthleteId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddAthleteToCoach_WhenCalledWithNonExistentAthleteID_ReturnsAddAthleteToCoachResponseDTOWithSuccessFalse()
        {
            // Arrange
            int existentCoachId = 1;
            int nonExistentAthleteId = 5;
            
            // Act
            var result = await _sut.AddAthleteToCoach(existentCoachId, nonExistentAthleteId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddAthleteToCoach_WhenCalledWithExistingIDsButAthleteAlreadyHasCoach_ReturnsAddAthleteToCoachResponseDTOWithSuccessFalse()
        {
            // Arrange
            int existentCoachId = 1;
            int existentAthleteId = 1;

            // Act
            var result = await _sut.AddAthleteToCoach(existentCoachId, existentAthleteId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddAthleteToCoach_WhenCalledWithExistingIDs_ReturnsAddAthleteToCoachResponseDTOWithSuccessTrue()
        {
            // Arrange
            int existentCoachId = 1;
            int existentAthleteId = 2;

            // Act
            var result = await _sut.AddAthleteToCoach(existentCoachId, existentAthleteId);

            // Assert
            Assert.True(result.Success);
        }
        #endregion

        #region Details
        [Fact]
        public async void Details_WhenCalledWithNonExistentCoachId_ReturnsAthleteDetailsDTOWithSuccessFalse()
        {
            // Arrange
            int nonExistentCoachId = 5;

            // Act
            var result = await _sut.Details(nonExistentCoachId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void Details_WhenCalledWithExistentCoachId_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange
            int existentCoachId = 1;

            // Act
            var result = await _sut.Details(existentCoachId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Coach", result.LastName);

            Assert.Single(result.Athletes);
        }
        #endregion

        #region EditDetails
        [Fact]
        public async void EditDetails_WhenCalledWithNonExistentCoachId_ReturnsAthleteDetailsDTOWithSuccessFalse()
        {
            // Arrange
            int nonExistentCoachId = 5;
            var editDetailsDTO = new EditDetailsDTO()
            {
                FirstName = "New first name"
            };

            // Act
            var result = await _sut.EditDetails(nonExistentCoachId, editDetailsDTO);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void EditDetailsd_WhenCalledWithExistentCoachId_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange   
            int existentCoachId = 1;
            var editDetailsDTO = new EditDetailsDTO()
            {
                FirstName = "New first name"
            };

            // Act
            var result = await _sut.EditDetails(existentCoachId, editDetailsDTO);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("New first name", result.FirstName);
            Assert.Equal("Coach", result.LastName);

            Assert.Single(result.Athletes);
        }
        #endregion

        #region Delete
        [Fact]
        public async void Delete_WhenCalledWithNonExistentCoachId_ReturnsDeleteCoachDTOWithSuccessFalseAndError()
        {
            // Arrange
            int nonExistentCoachId = 5;

            // Act
            var result = await _sut.Delete(nonExistentCoachId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void Delete_WhenCalledWithExistentCoachId_ReturnsDeleteCoachDTOWithSuccessTrue()
        {
            // Arrange
            int existentCoachId = 1;

            // Act
            var result = await _sut.Delete(existentCoachId);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async void Delete_WhenCalledWithExistentCoachId_DeletesCoachAndSubsequentCallsForCoachFail()
        {
            // Arrange
            int existentCoachId = 1;

            // Act
            // delete athlete
            await _sut.Delete(existentCoachId);

            // try to get that same athlete
            var result = await _sut.Details(existentCoachId);

            // Assert
            Assert.False(result.Success);
        }
        #endregion
    }
}
