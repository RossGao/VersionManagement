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
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<VersionDetail>()
                .HasOne(vd => vd.Version).WithMany(vi => vi.Detailes);
        }

        public DbSet<VersionInfo> Versions { get; set; }

        public DbSet<VersionDetail> Details { get; set; }
    }
}
