using Microsoft.EntityFrameworkCore;
using ICMS.Models;

namespace ICMS.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options
        ) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>()
                .Property(a => a.Status)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}