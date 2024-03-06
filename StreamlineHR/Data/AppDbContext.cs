using Microsoft.EntityFrameworkCore;
using StreamlineHR.Entities;

namespace StreamlineHR.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Applied> Applieds { get; set; }

    }
}
