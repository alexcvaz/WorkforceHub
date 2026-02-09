using Microsoft.EntityFrameworkCore;
using WorkforceHub.Server.Domain.Entities;

namespace WorkforceHub.Server.Infrastructure.Data
{
    public class WorkforceHubDbContext : DbContext
    {
        public WorkforceHubDbContext(DbContextOptions<WorkforceHubDbContext> options)
            : base(options)
        {
        }

        public DbSet<Punch> Punches { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Punch>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Timestamp)
                    .IsRequired();

                entity.Property(e => e.Type)
                    .IsRequired();

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(45);

                entity.Property(e => e.UserAgent)
                    .HasMaxLength(500);

                entity.HasIndex(e => new { e.UserId, e.Timestamp })
                    .HasDatabaseName("IX_Punch_UserId_Timestamp");
            });
        }
    }
}
