namespace TransitStopApp.Server.Interfaces;

/// <summary>
/// Contains operations for converting times represented as TimeOnly
/// and minutes since midnight.
/// </summary>
public interface IMinuteOfDayConverter
{
    /// <summary>
    /// Get the minutes since midnight given a time
    /// </summary>
    /// <param name="time">TimeOnly to convert</param>
    /// <returns>An integer representing minutes since midnight</returns>
    public int TimeOnlyToMinuteOfDay(TimeOnly time);

    /// <summary>
    /// Converts an integer representing the minutes of the day into a TimeOnly.
    /// </summary>
    /// <param name="minuteOfDay">
    /// Minutes since midnight. Must be between 0 (00:00) and 1439 (23:59) inclusive. 
    /// </param>
    /// <returns>TimeOnly representing the minuteOfDay value.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when a minuteOfDay is not within the valid range.
    /// </exception>
    public TimeOnly MinuteOfDayToTimeOnly(int minuteOfDay);
}
