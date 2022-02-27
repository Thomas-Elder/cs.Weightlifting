using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserContext: IdentityDbContext<ApplicationUser>
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
