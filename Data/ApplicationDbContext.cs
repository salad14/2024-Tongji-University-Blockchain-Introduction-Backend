using Microsoft.EntityFrameworkCore;
using StudentCompetitionAPI.Models;

namespace StudentCompetitionAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CompetitionAward> CompetitionAwards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 配置表名
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<CompetitionAward>().ToTable("CompetitionAwards");

            // 配置关系
            modelBuilder.Entity<CompetitionAward>()
                .HasOne(ca => ca.User)
                .WithMany()
                .HasForeignKey(ca => ca.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
