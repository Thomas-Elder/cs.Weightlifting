using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DatabaseContext: IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
