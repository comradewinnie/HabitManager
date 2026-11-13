using Microsoft.EntityFrameworkCore;
using HabitManager.Domain;

namespace HabitManager.Data
{
    // saģenerēja MI
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Habit> Habits => Set<Habit>();
        public DbSet<CompletionRecord> CompletionRecords => Set<CompletionRecord>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()  // saite User - Habit
                .HasMany(u => u.Habits)
                .WithOne(h => h.User)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Habit>()  // saite Habit - CompletionRecord
                .HasMany(h => h.History)
                .WithOne(c => c.Habit)
                .HasForeignKey(c => c.HabitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Habit>()  // Habit lauku konfigurācija
                .Property(h => h.Title)
                .IsRequired()
                .HasMaxLength(30);

            modelBuilder.Entity<Habit>()
                .Property(h => h.UserId)
                .IsRequired();

            modelBuilder.Entity<Habit>()
                .Property(h => h.Description)
                .IsRequired(false);

            modelBuilder.Entity<Habit>()
                .Property(h => h.Status)
                .IsRequired();

            modelBuilder.Entity<Habit>()
                .Property(h => h.Periodicity)
                .IsRequired();

            modelBuilder.Entity<Habit>()
                .Property(h => h.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<Habit>()
                .Property(h => h.UpdatedAt)
                .IsRequired();

            modelBuilder.Entity<CompletionRecord>()  // CompletionRecord lauku konfigurācija
                .Property(c => c.HabitId)
                .IsRequired();

            modelBuilder.Entity<CompletionRecord>()
                .Property(c => c.CompletedAt)
                .IsRequired();
        }
    }
}
