
using VizGuideBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace VizGuideBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Script> Scripts { get; set; }
        public DbSet<BaseType> BaseTypes { get; set; }
        public DbSet<MemberProcedure> MemberProcedures { get; set; }
        public DbSet<Propertie> Properties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
    }
}

