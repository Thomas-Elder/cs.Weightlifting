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

        #region AddAthleteToCoach
        [Fact]
        public async void AddAthleteToCoach_WhenCalledWithNonExistentUserID_ReturnsAddAthleteToCoachResponseDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.AddAthleteToCoach("2", 1);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddAthleteToCoach_WhenCalledWithNonExistentAthleteID_ReturnsAddAthleteToCoachResponseDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.AddAthleteToCoach("1", 2);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddAthleteToCoach_WhenCalledWithExistingIDs_ReturnsAddAthleteToCoachResponseDTOWithSuccessTrue()
        {
            // Arrange

            // Act
            var result = await _sut.AddAthleteToCoach("1", 1);

            // Assert
            Assert.True(result.Success);
        }
        #endregion

        #region DetailsByApplicationUserId
        [Fact]
        public async void DetailsByApplicationUserId_WhenCalledWithNonExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.DetailsByApplicationUserId("2");

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void DetailsByApplicationUserId_WhenCalledWithExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange

            // Act
            var result = await _sut.DetailsByApplicationUserId("1");

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Coach", result.LastName);

            Assert.Single(result.Athletes);
        }
        #endregion

        #region DetailsByCoachId
        [Fact]
        public async void DetailsByCoachId_WhenCalledWithNonExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.DetailsByCoachId(2);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void DetailsByCoachId_WhenCalledWithExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange

            // Act
            var result = await _sut.DetailsByCoachId(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Coach", result.LastName);

            Assert.Single(result.Athletes);
        }
        #endregion

        #region EditDetailsByApplicationUserId
        [Fact]
        public async void EditDetailsByApplicationUserId_WhenCalledWithNonExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessFalse()
        {
            // Arrange
            var editDetailsDTO = new EditDetailsDTO()
            {
                FirstName = "New first name"
            };

            // Act
            var result = await _sut.EditDetailsByApplicationUserId("2", editDetailsDTO);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void EditDetailsByApplicationUserId_WhenCalledWithExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange
            var editDetailsDTO = new EditDetailsDTO()
            {
                FirstName = "New first name"
            };

            // Act
            var result = await _sut.EditDetailsByApplicationUserId("1", editDetailsDTO);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("New first name", result.FirstName);
            Assert.Equal("Coach", result.LastName);

            Assert.Single(result.Athletes);
        }
        #endregion

        #region EditDetailsByCoachId
        [Fact]
        public async void EditDetailsByCoachId_WhenCalledWithNonExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessFalse()
        {
            // Arrange
            var editDetailsDTO = new EditDetailsDTO()
            {
                FirstName = "New first name"
            };

            // Act
            var result = await _sut.EditDetailsByCoachId(2, editDetailsDTO);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void EditDetailsByCoachId_WhenCalledWithExistentAthleteApplicationUserId_ReturnsAthleteDetailsDTOWithSuccessTrue()
        {
            // Arrange
            var editDetailsDTO = new EditDetailsDTO()
            {
                FirstName = "New first name"
            };

            // Act
            var result = await _sut.EditDetailsByCoachId(1, editDetailsDTO);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("New first name", result.FirstName);
            Assert.Equal("Coach", result.LastName);

            Assert.Single(result.Athletes);
        }
        #endregion
    }
}
