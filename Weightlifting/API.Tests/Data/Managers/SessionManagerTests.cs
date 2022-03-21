using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

using API.Data;
using API.Data.Models;
using API.Data.Managers;
using API.Data.Managers.Interfaces;
using API.DTOs.Sessions;

namespace API.Tests.Data.Managers
{
    public class SessionManagerTests
    {
        ISessionsManager _sut;

        WeightliftingContext mock_WeightliftingContext;

        public SessionManagerTests()
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

            _sut = new SessionsManager(mock_WeightliftingContext);
        }

        #region AddSession
        [Fact]
        public async void AddSession_WhenCalledWithNonExistingAthleteId_ReturnsAddSessionResponseDTOWithSuccessFalse()
        {
            // Arrange
            var addSessionDTO = new AddSessionDTO()
            {
                AthleteId = 2,
                Date = new DateTime(2022, 1, 2)
            };

            // Act
            var result = await _sut.AddSession(addSessionDTO);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void AddSession_WhenCalledWithExistingAthleteId_ReturnsAddSessionResponseDTOWithSuccessTrue()
        {
            // Arrange
            var addSessionDTO = new AddSessionDTO()
            {
                AthleteId = 1,
                Date = new DateTime(2022, 1, 2)
            };

            // Act
            var result = await _sut.AddSession(addSessionDTO);

            // Assert
            Assert.True(result.Success);
        }
        #endregion

        #region Details
        [Fact]
        public async void Details_WhenCalledWithNonExistentSessionId_ReturnsDetailsResponseDTOWithSuccessFalse()
        {
            // Arrange

            // Act
            var result = await _sut.Details(2);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void Details_WhenCalledWithExistentSessionId_ReturnsDetailsResponseDTOWithSuccessTrue()
        {
            // Arrange

            // Act
            var result = await _sut.Details(1);

            //Assert
            Assert.True(result.Success);
        }
        #endregion

        #region EditDetails
        [Fact]
        public async void EditDetails_WhenCalledWithNonExistentSessionId_ReturnsDetailsResponseDTOWithSuccessFalse()
        {
            // Arrange
            var editSessionDetailsDTO = new EditSessionDetailsDTO()
            {
                SessionId = 2,
                Date = new DateTime(2022, 1, 2)
            };

            // Act
            var result = await _sut.EditDetails(editSessionDetailsDTO);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async void EditDetails_WhenCalledWithExistentSessionId_ReturnsDetailsResponseDTOWithSuccessTrueAndDetailsAreUpdated()
        {
            // Arrange
            var editSessionDetailsDTO = new EditSessionDetailsDTO()
            {
                SessionId = 1,
                Date = new DateTime(2022, 1, 2)
            };

            // Act
            var result = await _sut.EditDetails(editSessionDetailsDTO);
            var updatedSession = await _sut.Details(1);

            //Assert
            Assert.True(result.Success);
            Assert.Equal(editSessionDetailsDTO.Date, result.Date);
        }
        #endregion
    }
}
