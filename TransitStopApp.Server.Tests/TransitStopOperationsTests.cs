using Microsoft.EntityFrameworkCore;
using TransitStopApp.Server.Models;
using TransitStopApp.Server.Utility;

namespace TransitStopApp.Server.Tests;

public class TransitStopOperationsTests
{
    /// <summary>
    /// Test that all stops are returned in order
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAllStopsTest()
    {
        var inputStops = TestStopDataFactory.GetStops(5)
            .Reverse(); // reverse to test ordering logic
        var context = GetContext(inputStops, []);
        var stopOperations = new TransitStopOperations(context);

        var result = await stopOperations.GetAllStopsAsync();

        Assert.Equal(5, result.Count);

        // assert results are ordered by StopOrder
        var prev = result.First();
        foreach(var st in result.Skip(1))
        {
            Assert.True(st.StopOrder >= prev.StopOrder);
            prev = st;
        }
    }

    /// <summary>
    /// Test the next StopTime is returned including returning the next day's earliest StopTime if 
    /// none are left for the current day.
    /// </summary>
    /// <param name="testCase"></param>
    /// <returns></returns>
    [Theory]
    [MemberData(nameof(GetNextStopTimeTestData))]
    public async Task GetNextStopTimeTest(StopTimeTestCase testCase)
    {
        var stops = TestStopDataFactory.GetStops(1);
        var stopTimes = TestStopDataFactory.GetStopTimes(testCase.StopId, testCase.StopTimes);
        var context = GetContext(stops, stopTimes);
        var stopOperations = new TransitStopOperations(context);

        var actual = await stopOperations.GetNextStopTimeAsync(testCase.StopId, testCase.CurrentMinute);

        Assert.Equal(testCase.Expected, actual);
    }

    public record StopTimeTestCase(int StopId, int[] StopTimes, int CurrentMinute, int Expected);

    public static IEnumerable<object[]> GetNextStopTimeTestData()
    {
        return new List<object[]>
        {
            new object[] 
            { // test fetching next future stop time
                new StopTimeTestCase(1, [100, 200, 300], 150, 200)
            }, 
            new object[] 
            { // test fetch earliest from next day when no more available today
                new StopTimeTestCase(1, [100, 200, 300], 400, 100)
            },
            new object[] 
            { // test no stops returns -1
                new StopTimeTestCase(1, Array.Empty<int>(), 100, -1)
            },
            new object[] 
            { // test stop on current minute, get the next instead
                new StopTimeTestCase(1, [100, 200, 300], 200, 300)
            }
        };
    }
    private static TransitStopDbContext GetContext(IEnumerable<Stop> stops,
        IEnumerable<StopTime> times)
    {
        var options = new DbContextOptionsBuilder<TransitStopDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new TransitStopDbContext(options);

        context.Stops.AddRange(stops);
        context.StopTimes.AddRange(times);
        context.SaveChanges();

        return context;
    }
}
