using Microsoft.EntityFrameworkCore;

namespace TransitStopApp.Server.Utility;

/// <summary>
/// Initialization logic for SQLite DB.
/// </summary>
public static class TransitStopDbInitializer
{
    /// <summary>
    /// If the SQLite DB does not exist, create the .db file under the folder specified
    /// in connectionString. If no data in the DB, insert sample data into the Stop and 
    /// StopTime tables.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static async Task Initialize(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TransitStopDbContext>();
        optionsBuilder.UseSqlite(connectionString);

        var context = new TransitStopDbContext(optionsBuilder.Options);

        // Create DB according to DbContext schema if it doesn't exist
        await context.Database.EnsureCreatedAsync();

        if (context.Stops.Any()) return;

        var stops = SampleDataGenerator.GetStops();
        context.Stops.AddRange(stops);
        context.SaveChanges();
        
        var stopTimes = SampleDataGenerator.GetStopTimes();
        context.StopTimes.AddRange(stopTimes);
        context.SaveChanges();
    }
}
