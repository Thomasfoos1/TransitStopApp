using TransitStopApp.Server.Interfaces;

namespace TransitStopApp.Server.Utility;

/// <summary>
/// Fetches the current time of day in Pacific Standard Time.
/// </summary>
public class CurrentTimeFetcherPst : ICurrentTimeFetcher
{
    public TimeOnly Fetch(DateTime utcNow)
    {
        var pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        var pstTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, pstZone);
        return TimeOnly.FromDateTime(pstTime);
    }
}
