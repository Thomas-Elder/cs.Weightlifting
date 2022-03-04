using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class WeightliftingContext : DbContext
    {
        public DbSet<Athlete> Athletes { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public WeightliftingContext(DbContextOptions<WeightliftingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Athlete>()
                .HasOne(a => a.Coach)
                .WithMany(c => c.Athletes);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Athlete)
                .WithMany(a => a.Sessions);
        }
    }
}
