using TransitStopApp.Server.Utility;

namespace TransitStopApp.Server.Tests;

public class MinuteOfDayConverterTests
{
    private readonly MinuteOfDayConverter _converter = new();

    /// <summary>
    /// Test TimeOnly is successfully converted to minute of day
    /// </summary>
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(10, 32, 632)]
    [InlineData(23, 59, 1439)]
    public void TimeOnlyToMinuteOfDayTest(int hour, int minute, int expected)
    {
        var actual = _converter.TimeOnlyToMinuteOfDay(new TimeOnly(hour, minute));
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Test that valid minute of day is successfully converted to TimeOnly
    /// </summary>
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(827, 13, 47)]
    [InlineData(1439, 23, 59)]
    public void MinuteOfDayToTimeOnlyValidTests(int minuteOfDay, int expectedHour, int expectedMinute)
    {
        var expectedTime = new TimeOnly(expectedHour, expectedMinute);
        var actualTime = _converter.MinuteOfDayToTimeOnly(minuteOfDay);
        Assert.Equal(expectedTime, actualTime);
    }

    /// <summary>
    /// Test invalid minute of day throws an InvalidOperationException
    /// </summary>
    [Theory]
    [InlineData(-10)]
    [InlineData(1440)]
    public void MinuteOfDayToTimeOnlyInvalidTests(int minuteOfDay)
    {
        Assert.Throws<InvalidOperationException>(() => 
        _converter.MinuteOfDayToTimeOnly(minuteOfDay));
    }
}