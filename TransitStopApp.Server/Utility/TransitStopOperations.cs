using Microsoft.EntityFrameworkCore;
using TransitStopApp.Server.Interfaces;
using TransitStopApp.Server.Models;

namespace TransitStopApp.Server.Utility;


/// <summary>
/// Iplementation of ITransitStopOperations that uses Entity Framework
/// to handle CRUD operations. 
/// </summary>
public class TransitStopOperations(TransitStopDbContext dbContext)
    : ITransitStopOperations
{
    public async Task<IEnumerable<Stop>> GetAllStopsAsync()
    {
        return await dbContext.Stops
            .OrderBy(s => s.StopOrder)
            .ToListAsync();
    }

    public async Task<int> GetNextStopTimeAsync(int stopId, int currentMinuteOfDay)
    {
        var stopTime = await dbContext.StopTimes
            .Where(t => t.StopId == stopId && t.StopMinuteOfDay > currentMinuteOfDay)
            .OrderBy(t => t.StopMinuteOfDay)
            .FirstOrDefaultAsync();

        stopTime ??= await dbContext.StopTimes
            .Where(t => t.StopId == stopId)
            .OrderBy(t => t.StopMinuteOfDay)
            .FirstOrDefaultAsync();

        return stopTime is not null ? 
            stopTime.StopMinuteOfDay : -1;
    }
}
