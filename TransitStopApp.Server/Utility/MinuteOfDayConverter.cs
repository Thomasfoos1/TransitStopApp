using TransitStopApp.Server.Interfaces;

namespace TransitStopApp.Server.Utility;

/// <summary>
/// Implementation of IMinuteOfDayConverter that converts
/// between TimeOnly and minute of day since midnight.
/// </summary>
public class MinuteOfDayConverter : IMinuteOfDayConverter
{
    public int TimeOnlyToMinuteOfDay(TimeOnly time) =>
        time.Hour * 60 + time.Minute;

    public TimeOnly MinuteOfDayToTimeOnly(int minuteOfDay)
    {
        const int maxValidMin = 60 * 24 - 1;
        if (minuteOfDay < 0 || minuteOfDay > maxValidMin)
            throw new InvalidOperationException($"The MinuteOfDay {minuteOfDay}, is " +
                $"not within the valid range of 0 and {maxValidMin}");

        var hour = minuteOfDay / 60;
        var minute = minuteOfDay % 60;
        return new TimeOnly(hour, minute);
    }
}
