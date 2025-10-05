using TransitStopApp.Server.Models;

namespace TransitStopApp.Server.Utility;

/// <summary>
/// Contains methods for generating sample Stop and StopTime data.
/// </summary>
public static class SampleDataGenerator
{
    /// <summary>
    /// Returns a pre-defined list of Stops on a sample route.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Stop> GetStops()
    {
        List<Stop> stops =
        [
            new () { Name = "Beaudry Ave & 3rd St", StopOrder = 1 },
            new () { Name = "Flower St & 7th St", StopOrder = 2 },
            new () { Name = "Flower St & Olympic Blvd", StopOrder = 3 },
            new () { Name = "Figueroa St & Venice Blvd", StopOrder = 4 },
            new () { Name = "Figueroa St & 30th St", StopOrder = 5 },
            new () { Name = "Exposition Blvd & Trousdale Pkwy", StopOrder = 6 },
        ];
        return stops;
    }

    /// <summary>
    /// Generate StopTimes for an entire day from 6 AM to 9 PM for all pre-defined stops.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<StopTime> GetStopTimes()
    {
        const int startMinute = 60 * 6; // 6 AM
        const int endMinute = 60 * 21; // 9 PM
        const int stopIntervalMinutes = 30;
        const int travelTimeMinutes = 10;

        // StopId to first StopMinuteOfDay
        // 2 buses on the route for this example which start at StopId 1 & 4 at 6 AM.
        var firstStopTimes = new Dictionary<int, int>
        {
            { 1, startMinute },
            { 2, startMinute + travelTimeMinutes },
            { 3, startMinute + travelTimeMinutes * 2},
            { 4, startMinute },
            { 5, startMinute + travelTimeMinutes },
            { 6, startMinute + travelTimeMinutes * 2},
        };

        var stopTimes = new List<StopTime>();

        for (var stopId = 1; stopId <= 6; stopId++)
        {
            var firstStopMinute = firstStopTimes[stopId];
            for (var st = firstStopMinute; st < endMinute; st += stopIntervalMinutes)
            {
                stopTimes.Add(new StopTime
                {
                    StopId = stopId,
                    StopMinuteOfDay = st
                });
            }
        }

        return stopTimes;
    }
}