namespace Torshia.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class TorshiaDbContext : DbContext
    {
        public TorshiaDbContext()
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<TaskSector> TaskSectors { get; set; }

        public DbSet<Report> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}