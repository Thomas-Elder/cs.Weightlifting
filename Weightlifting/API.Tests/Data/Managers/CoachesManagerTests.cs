﻿using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

using API.Data;
using API.Data.Models;
using API.Data.Managers;

namespace API.Tests.Data.Managers
{
    public class CoachesManagerTests : IDisposable
    {
        private readonly CoachesManager _sut;

        private readonly WeightliftingContext mock_WeightliftingContext;

        public CoachesManagerTests()
        {
            var weightliftingOptions = new DbContextOptionsBuilder<WeightliftingContext>()
                .UseInMemoryDatabase(databaseName: "WeightliftinTest")
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

        #region GetAthletes
        #endregion
    }
}
