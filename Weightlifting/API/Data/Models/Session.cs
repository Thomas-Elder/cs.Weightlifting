namespace API.Data.Models
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<Exercise>? Exercises { get; set; }

        #region Navigation Properties
        public Athlete? Athlete { get; set; }
        public int AthleteId { get; set; }
        #endregion
    }
}
