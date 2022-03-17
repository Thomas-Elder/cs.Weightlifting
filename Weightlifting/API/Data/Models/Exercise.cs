namespace API.Data.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<Set>? Sets { get; set; }

        #region Navigation Properties
        public Session? Session { get; set; }
        public int SessionId { get; set; }
        #endregion
    }
}
