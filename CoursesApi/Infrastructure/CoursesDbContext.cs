using Microsoft.EntityFrameworkCore;

namespace CoursesApi.Models
{
    public class CoursesDbContext : DbContext
    {
        public CoursesDbContext(DbContextOptions<CoursesDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasMany(x => x.Chapters)
                .WithOne(y => y.Course);

            modelBuilder.Entity<Chapter>()
                .HasMany(x => x.Lessons)
                .WithOne(y => y.Chapter);

            modelBuilder.Entity<User>()
                .HasMany(x => x.LessonCompletions)
                .WithOne(y => y.User);

            modelBuilder.Entity<User>()
                .HasMany(x => x.AchievementProgresses)
                .WithOne(y => y.User);

            modelBuilder.Entity<LessonCompletion>()
                .HasOne(x => x.Lesson);

            modelBuilder.Entity<AchievementProgress>()
                .HasOne(x => x.Achievement);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite("Data Source=Courses.db");
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<LessonCompletion> LessonCompletions { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<AchievementProgress> AchievementProgresses { get; set; }
    }
}
