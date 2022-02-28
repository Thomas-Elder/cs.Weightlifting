namespace API.Data.Models
{
    public class Coach
    {
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public IEnumerable<Athlete>? Athletes { get; set; }
    }
}
