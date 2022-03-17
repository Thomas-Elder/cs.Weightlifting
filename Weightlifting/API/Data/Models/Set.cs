namespace API.Data.Models
{
    public class Set
    {
        public int Id { get; set; }
        public int Weight { get; set; }
        public int SuccessfulRepetitions { get; set; }
        public int FailedRepetitions { get; set; }
        public int TotalRepetitions
        {
            get { return SuccessfulRepetitions + FailedRepetitions; }
        }

        #region Navigation Properties
        public Exercise? Exercise { get; set; }
        public int ExerciseId { get; set; }
        #endregion
    }
}
