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
    }
}