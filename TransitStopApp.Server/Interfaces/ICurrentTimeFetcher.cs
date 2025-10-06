namespace TransitStopApp.Server.Interfaces;

/// <summary>
/// Fetches the current time of day.
/// </summary>
public interface ICurrentTimeFetcher
{
    /// <summary>
    /// Fetch the current time of day.
    /// </summary>
    /// <param name="utcNow">Current DateTime in UTC</param>
    /// <returns>A TimeOnly representing the current time of day.</returns>
    public TimeOnly Fetch(DateTime utcNow);
}
