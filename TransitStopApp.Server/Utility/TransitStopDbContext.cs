using Microsoft.EntityFrameworkCore;
using TransitStopApp.Server.Models;

namespace TransitStopApp.Server.Utility;

/// <summary>
/// EF Core DbContext for TransitStop DB. Contains tables for bus stops and 
/// their scheduled stop times. 
/// </summary>
public class TransitStopDbContext : DbContext
{
    public TransitStopDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Stop> Stops => Set<Stop>();
    public DbSet<StopTime> StopTimes => Set<StopTime>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Foreign key on StopId -> Stop.Id
        modelBuilder.Entity<StopTime>()
            .HasOne<Stop>()                
            .WithMany()                    
            .HasForeignKey(t => t.StopId) 
            .OnDelete(DeleteBehavior.Cascade);

        // Index on StopId and StopMinuteOfDay for efficient querying of next stop time
        modelBuilder.Entity<StopTime>()
            .HasIndex(t => new { t.StopId, t.StopMinuteOfDay })
            .HasDatabaseName("IX_StopTime_StopId_StopMinuteOfDay");
    }
}
