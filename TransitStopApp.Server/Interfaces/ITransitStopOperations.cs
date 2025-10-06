using TransitStopApp.Server.Models;

namespace TransitStopApp.Server.Interfaces;

/// <summary>
/// Contains CRUD operations on TransitStop models. 
/// </summary>
public interface ITransitStopOperations
{
    /// <summary>
    /// Get all stops ordered by StopOrder
    /// </summary>
    /// <returns>A collection of all stops</returns>
    Task<ICollection<Stop>> GetAllStopsAsync();

    /// <summary>
    /// Find the next stop time for a given stopId and current time.
    /// If no stop is available for the current day, find the earliest stop
    /// of the next day.
    /// </summary>
    /// <param name="stopId">ID of the stop to query</param>
    /// <param name="currentMinuteOfDay">
    /// The current time in minutes since midnight
    /// </param>
    /// <returns>
    /// An integer representing the next stop minute of day, or -1 if not found
    /// </returns>
    Task<int> GetNextStopTimeAsync(int stopId, int currentMinuteOfDay);
}
