using ApiNumerica.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNumerica.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Job> Jobs => Set<Job>();

    public DbSet<JobIteration> JobIterations => Set<JobIteration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Job>()
            .HasMany(j => j.Iterations)
            .WithOne(i => i.Job)
            .HasForeignKey(i => i.JobId);

        modelBuilder.Entity<Job>()
            .Property(j => j.Status)
            .HasMaxLength(30);

        modelBuilder.Entity<Job>()
            .Property(j => j.Method)
            .HasMaxLength(80);
    }
}