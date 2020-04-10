using Microsoft.EntityFrameworkCore;

namespace AIMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Solution> Solution { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Database> Databases { get; set; }
        public DbSet<Server> ServerInfo { get; set; }
        public DbSet<SolutionApplication> Solution_Application { get; set; }
        public DbSet<SolutionDatabase> Solution_Database { get; set; }
        public DbSet<ApplicationServer> Application_ServerInfo { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SolutionApplication>().HasKey(sa => new { sa.SolutionId, sa.ApplicationId });
            modelBuilder.Entity<SolutionDatabase>().HasKey(sd => new { sd.SolutionId, sd.DatabaseId });
            modelBuilder.Entity<ApplicationServer>().HasKey(aps => new { aps.ApplicationId, aps.ServerInformationId});
        }
    }
}
