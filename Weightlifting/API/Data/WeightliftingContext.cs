using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class WeightliftingContext : DbContext
    {
        public WeightliftingContext(DbContextOptions<WeightliftingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
