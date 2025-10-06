using TransitStopApp.Server.Utility;

namespace TransitStopApp.Server.Tests;

public class CurrentTimeFetcherPstTests
{
    /// <summary>
    /// Confirm the input date utc date is converted to UTC
    /// </summary>
    [Fact]
    public void ConvertsUtcToPstTest()
    {
        var utcDateTime = new DateTime(2025, 10, 5, 17, 23, 0, DateTimeKind.Utc);
        var fetcher = new CurrentTimeFetcherPst();

        var pstTime = fetcher.Fetch(utcDateTime);

        Assert.Equal(10, pstTime.Hour);
        Assert.Equal(23, pstTime.Minute);
    }
}
