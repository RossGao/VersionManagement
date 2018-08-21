using Microsoft.EntityFrameworkCore;
using VersionManagement.Models;

namespace VersionManagement.Repositories
{
    public class VersionContext : DbContext
    {
        public VersionContext(DbContextOptions<VersionContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<VersionInfo> Versions { get; set; }
    }
}
