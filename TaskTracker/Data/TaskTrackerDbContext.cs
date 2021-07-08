using Microsoft.EntityFrameworkCore;

using TaskTracker.Data.Models;


namespace TaskTracker.Data
{
    public class TaskTrackerDbContext : DbContext
    {
        public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .HasOne<Project>(t => t.Project)
                .WithMany(p => p.Tasks);
        }
    }
}