﻿using Microsoft.EntityFrameworkCore;

using API.Data.Managers.Interfaces;

using DTO.Coaches;

namespace API.Data.Managers
{
    /// <summary>
    /// Manages the updating and reading of Coach records
    /// </summary>
    public class CoachesManager : ICoachesManager
    {
        private readonly WeightliftingContext _weightliftingContext;
        public CoachesManager(WeightliftingContext weightliftingContext)
        {
            _weightliftingContext = weightliftingContext;
        }

        /// <summary>
        /// Gets the coach id for the Application User Id passed in
        /// </summary>
        /// If there is an Coach with that Application User Id it sets coachId to  
        /// the value of Coach.Id, and returns true.
        /// If there is not an Coach with that Application User Id, coachId is 0,
        /// and the method returns false.
        /// <param name="applicationUserId"></param>
        /// <param name="coachId"></param>
        /// <returns>
        /// True if the given coachId exists in the context, and coachId is set accordingly
        /// </returns>
        public bool GetCoachId(string applicationUserId, out int coachId)
        {
            var coach = _weightliftingContext.Coaches.FirstOrDefault(c => c.ApplicationUserId == applicationUserId);

            if (coach == null)
            {
                coachId = 0;
                return false;
            }

            coachId = coach.Id;
            return true;
        }

        /// <summary>
        /// Adds the Athlete with the given athleteId to the Coach with the given coachId. 
        /// </summary>
        /// If the coachId does not exist, returns a AddAthleteToCoachResponseDTO with Success false.
        /// If the athleteId does not exist, returns a AddAthleteToCoachResponseDTO with Success false.
        /// If both ids exist, but the athlete is already assigned a coach, believe it or not, 
        /// returns a AddAthleteToCoachResponseDTO with Success false.
        /// If both ids exist, and the athlete is not already associated with coach, returns a 
        /// AddAthleteToCoachResponseDTO with Success true.
        /// <param name="coachId"></param>
        /// <param name="athleteId"></param>
        /// <returns>
        /// An AddAthleteToCoachResponseDTO with the result of the action.
        /// </returns>
        public async Task<AddAthleteToCoachResponseDTO> AddAthleteToCoach(int coachId, int athleteId)
        {
            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.Id == coachId);

            if (coach is null)
            {
                return new AddAthleteToCoachResponseDTO()
                {
                    Success = false,
                    Errors = new List<string>{ "Coach id doesn't exist" }
                };
            }

            var athlete = await _weightliftingContext.Athletes.FirstOrDefaultAsync(a => a.Id == athleteId);

            if (athlete is null)
            {
                return new AddAthleteToCoachResponseDTO()
                {
                    Success = false,
                    Errors = new List<string> { "Athlete id doesn't exist" }
                };
            }

            if (athlete.Coach is not null)
            {
                return new AddAthleteToCoachResponseDTO()
                {
                    Success = false,
                    Errors = new List<string> { "Athlete already has a coach" }
                };
            }

            athlete.CoachId = coach.Id;
            athlete.Coach = coach;

            _weightliftingContext.SaveChanges();

            return new AddAthleteToCoachResponseDTO()
            {
                Success = true
            };
        }

        /// <summary>
        /// Gets the Coach details associated with the given coachId.
        /// </summary>
        /// If the coachId is not associated with a Coach record, a CoachDetailsResponseDTO is returned
        /// with Success set to false, and an error message in the Error dictionary. 
        /// 
        /// If the coachId is associated with a Coach record, a CoachDetailsResponseDTO is returned with 
        /// details of the Coach record, and Success is set to true.
        /// <param name="coachId"></param>
        /// <returns>
        /// A CoachDetailsResponseDTO with the result of the action.
        /// </returns>
        public async Task<CoachDetailsResponseDTO> Details(int coachId)
        {
            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.Id == coachId);

            if (coach is null)
            {
                return new CoachDetailsResponseDTO()
                {
                    Success = false,
                    Errors = new List<string> { "No coach with that id exists" }
                };
            }

            var athletes = await _weightliftingContext.Athletes
                .Where(a => a.CoachId == coach.Id)
                .ToListAsync();

            var athleteDetailsDTOs = new List<AthleteDetailsDTO>();

            foreach (var athlete in athletes)
            {
                athleteDetailsDTOs.Add(new AthleteDetailsDTO()
                {
                    AthleteId = athlete.Id,
                    FirstName = athlete.FirstName,
                    LastName = athlete.LastName
                });
            }

            return new CoachDetailsResponseDTO()
            {
                Success = true,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                Athletes = athleteDetailsDTOs
            };
        }

        /// <summary>
        /// Updates the details of a Coach record.
        /// </summary>
        /// If the coachId is not associated with a Coach record, a CoachDetailsResponseDTO is returned
        /// with Success set to false, and an error message in the Error dictionary.
        /// 
        /// If the coachId is associated with a Coach record, the record is updated with the details given
        /// and a CoachDetailsResponseDTO is returned with details of the Coach record, and Success is set to true.
        /// <param name="coachId"></param>
        /// <param name="editDetailsDTO"></param>
        /// <returns>
        /// A CoachDetailsResponseDTO with the result of the action.
        /// </returns>
        public async Task<CoachDetailsResponseDTO> EditDetails(int coachId, EditDetailsDTO editDetailsDTO)
        {
            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.Id == coachId);

            if (coach is null)
            {
                return new CoachDetailsResponseDTO()
                {
                    Success = false,
                    Errors = new List<string> { "No coach with that user id exists" }
                };
            }

            coach.FirstName = editDetailsDTO.FirstName ?? coach.FirstName;
            coach.LastName = editDetailsDTO.LastName ?? coach.LastName;

            _weightliftingContext.SaveChanges();

            var athletes = await _weightliftingContext.Athletes
                .Where(a => a.CoachId == coach.Id)
                .ToListAsync();

            var athleteDetailsDTOs = new List<AthleteDetailsDTO>();

            foreach (var athlete in athletes)
            {
                athleteDetailsDTOs.Add(new AthleteDetailsDTO()
                {
                    AthleteId = athlete.Id,
                    FirstName = athlete.FirstName,
                    LastName = athlete.LastName
                });
            }

            return new CoachDetailsResponseDTO()
            {
                Success = true,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                Athletes = athleteDetailsDTOs
            };
        }

        /// <summary>
        /// Deletes an Coach from the database.
        /// </summary>
        /// Returns a DeleteCoachDTO with Success flag set to false if the given coachId is not
        /// associated with an Coach in the db.
        /// Otherwise it will have Success set to true.
        /// <param name="coachId"></param>
        /// <returns>
        /// A DeleteCoachDTO with the result of the action.
        /// </returns>
        public async Task<DeleteCoachDTO> Delete(int coachId)
        {
            var coach = await _weightliftingContext.Coaches.FirstOrDefaultAsync(c => c.Id == coachId);

            if (coach is null)
            {
                return new DeleteCoachDTO()
                {
                    Success = false,
                    Errors = new List<string> { "No coach with that user id exists" }
                };
            }

            // else delete and return
            _weightliftingContext.Coaches.Remove(coach);
            _weightliftingContext.SaveChanges();

            return new DeleteCoachDTO()
            {
                Success = true
            };
        }
    }
}
