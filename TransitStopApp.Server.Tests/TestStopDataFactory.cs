using TransitStopApp.Server.Models;

namespace TransitStopApp.Server.Tests;

/// <summary>
/// Provides test data for Stops and StopTimes.
/// </summary>
public static class TestStopDataFactory
{
    public static IEnumerable<Stop> GetStops(int numStops)
    {
        return Enumerable.Range(1, numStops)
            .Select(i => new Stop
            {
                Id = i,
                StopOrder = i,
                Name = $"{i} and TestStreet"
            });
    }

    public static IEnumerable<StopTime> GetStopTimes(int stopId,
        IEnumerable<int> stopTimes)
    {
        return stopTimes.Select((st, idx) => new StopTime {
            Id = idx + 1,
            StopId = stopId,
            StopMinuteOfDay = st
        });
    }
}
