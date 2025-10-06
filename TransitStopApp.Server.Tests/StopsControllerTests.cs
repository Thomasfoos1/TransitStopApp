using Microsoft.AspNetCore.Mvc;
using Moq;
using TransitStopApp.Server.Controllers;
using TransitStopApp.Server.DTOs;
using TransitStopApp.Server.Interfaces;
using TransitStopApp.Server.Models;

namespace TransitStopApp.Server.Tests;

public class StopsControllerTests
{
    /// <summary>
    /// Test all stops are successfully returned.
    /// </summary>
    [Fact]
    public async Task GetAllStopsTest()
    {
        var stops = TestStopDataFactory.GetStops(3).ToList();
        var mockOps = new Mock<ITransitStopOperations>();
        mockOps.Setup(m => m.GetAllStopsAsync())
            .ReturnsAsync(stops);
        var stopOps = mockOps.Object;

        // No functionality needed for this test
        var timeFetcher = new Mock<ICurrentTimeFetcher>().Object;
        var timeConverter = new Mock<IMinuteOfDayConverter>().Object;

        var controller = new StopsController(stopOps, timeConverter, timeFetcher);

        var result = await controller.GetAllStops();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultStops = Assert.IsAssignableFrom<ICollection<Stop>>(okResult.Value);
        Assert.Equal(3, resultStops.Count);
    }

    /// <summary>
    /// Test success is returned when a next stop time is found.
    /// </summary>
    [Fact]
    public async Task GetNextStopTimeSuccess()
    {
        var expectedResult = "7:33 AM";

        var curTimeOnly = new TimeOnly(7, 15);
        var curMinute = 435;

        var nextTimeOnly = new TimeOnly(7, 33);
        var nextMinute = 453;

        var stopOps = GetStopOpsForNextStop(nextMinute);
        var timeConverter = GetMinuteOfDayConverter(curMinute, nextTimeOnly);
        var timeFetcher = GetCurrentTimeFetcher(curTimeOnly);
        var controller = new StopsController(stopOps, timeConverter, timeFetcher);

        var result = await controller.GetNextStopTime(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        var resultDto = Assert.IsAssignableFrom<NextStopResponse>(okResult.Value);
        Assert.Equal(expectedResult, resultDto.NextStop);
    }

    /// <summary>
    /// Test that when no stop is found, a NotFound result is returned.
    /// </summary>
    [Fact]
    public async Task GetNextStopTimeNotFound()
    {
        var curTimeOnly = new TimeOnly(7, 15);
        var curMinute = 435;

        var nextTimeOnly = new TimeOnly(7, 33);

        var stopOps = GetStopOpsForNextStop(-1);
        var timeConverter = GetMinuteOfDayConverter(curMinute, nextTimeOnly);
        var timeFetcher = GetCurrentTimeFetcher(curTimeOnly);
        var controller = new StopsController(stopOps, timeConverter, timeFetcher);

        var result = await controller.GetNextStopTime(1);

        var okResult = Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    private static ITransitStopOperations GetStopOpsForNextStop(int result)
    {
        var mockOps = new Mock<ITransitStopOperations>();
        mockOps.Setup(m => m.GetNextStopTimeAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(result);
        return mockOps.Object;
    }

    private static ICurrentTimeFetcher GetCurrentTimeFetcher(TimeOnly result)
    {
        var mockOps = new Mock<ICurrentTimeFetcher>();
        mockOps.Setup(m => m.Fetch(It.IsAny<DateTime>()))
            .Returns(result);
        return mockOps.Object;
    }

    private static IMinuteOfDayConverter GetMinuteOfDayConverter(int toMinuteResult,
        TimeOnly toTimeOnlyResult)
    {
        var mockOps = new Mock<IMinuteOfDayConverter>();
        mockOps.Setup(m => m.TimeOnlyToMinuteOfDay(It.IsAny<TimeOnly>()))
            .Returns(toMinuteResult);
        mockOps.Setup(m => m.MinuteOfDayToTimeOnly(It.IsAny<int>()))
            .Returns(toTimeOnlyResult);
        return mockOps.Object;
    }
}
