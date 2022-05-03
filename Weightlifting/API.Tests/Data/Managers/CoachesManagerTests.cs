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

            // Act
            var result = _sut.GetCoachId("5", out int coachId);

            // Assert
            Assert.False(result);
            Assert.Equal(0, coachId);
        }

        [Fact]
        public void GetCoachId_WhenCalledWithExistentUserID_ReturnsTrueAndSetsAthleteIdParameter()
        {
            // Arrange

            // Act
            var result = _sut.GetCoachId("1", out int coachId);

            // Assert
            Assert.True(result);
            Assert.Equal(1, coachId);
        }
        #endregion

        #region AddAthleteToCoach
        [Fact]
        public async void AddAthleteToCoach_WhenCalledWithNonExistentCoachID_ReturnsAddAthleteToCoachResponseDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.AddAthleteToCoach(5, 1);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddAthleteToCoach_WhenCalledWithNonExistentAthleteID_ReturnsAddAthleteToCoachResponseDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.AddAthleteToCoach(1, 2);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddAthleteToCoach_WhenCalledWithExistingIDs_ReturnsAddAthleteToCoachResponseDTOWithSuccessTrue()
        {
            // Arrange

            // Act
            var result = await _sut.AddAthleteToCoach(1, 1);

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
            Assert.Equal("Coach", result.LastName);

            Assert.Single(result.Athletes);
        }
        #endregion

        #region EditDetails
        [Fact]
        public async void EditDetails_WhenCalledWithNonExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessFalse()
        {
            // Arrange
            var editDetailsDTO = new EditDetailsDTO()
            {
                FirstName = "New first name"
            };

            // Act
            var result = await _sut.EditDetails(2, editDetailsDTO);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void EditDetailsd_WhenCalledWithExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange
            var editDetailsDTO = new EditDetailsDTO()
            {
                FirstName = "New first name"
            };

            // Act
            var result = await _sut.EditDetails(1, editDetailsDTO);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("New first name", result.FirstName);
            Assert.Equal("Coach", result.LastName);

            Assert.Single(result.Athletes);
        }
        #endregion
    }
}
