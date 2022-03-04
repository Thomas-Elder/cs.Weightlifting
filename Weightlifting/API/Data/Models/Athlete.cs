namespace API.Data.Models
{
    public class Athlete
    {
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        #region Navigation Properties
        public IEnumerable<Session>? Sessions { get; set; }

        public int? CoachId { get; set; }
        public Coach? Coach { get; set; }
        #endregion
    }
}
