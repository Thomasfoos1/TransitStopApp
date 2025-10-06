using Microsoft.AspNetCore.Mvc;
using TransitStopApp.Server.DTOs;
using TransitStopApp.Server.Interfaces;
using TransitStopApp.Server.Models;

namespace TransitStopApp.Server.Controllers;

/// <summary>
/// API endpoints for stop data and their next scheduled stop times.
/// </summary>
/// <param name="stopOperations">Service to hanlde Stop and StopTime CRUD operations</param>
/// <param name="timeConverter">Service to convert between TimeOnly and minute of day</param>
/// <param name="currentTimeFetcher">Service to fetch the current TimeOnly</param>
[ApiController]
[Route("api/[controller]")]
public class StopsController(ITransitStopOperations stopOperations,
    IMinuteOfDayConverter timeConverter, ICurrentTimeFetcher currentTimeFetcher)
    : ControllerBase
{
    /// <summary>
    /// Fetch all Stops ordered by StopOrder
    /// </summary>
    /// <returns>An ICollection of all the stops</returns>
    [HttpGet]
    public async Task<ActionResult<ICollection<Stop>>> GetAllStops()
    {
        var stops = await stopOperations.GetAllStopsAsync();
        return Ok(stops);
    }

    /// <summary>
    /// Fetch the next stopTime for a specific stop.
    /// </summary>
    /// <param name="stopId">Id of the Stop to query</param>
    /// <returns>
    /// A string of the next scheduled time in "HH:mm" format
    /// or 404 Not Found if no data on the next stop.
    /// </returns>
    [HttpGet("{stopId}/next")]
    public async Task<ActionResult<NextStopResponse>> GetNextStopTime(int stopId)
    {
        var currentTime = currentTimeFetcher.Fetch(DateTime.UtcNow);
        var minuteOfDay = timeConverter.TimeOnlyToMinuteOfDay(currentTime);
        var nextStopTime = await stopOperations
            .GetNextStopTimeAsync(stopId, minuteOfDay);

        if (nextStopTime < 0)
            return NotFound("Unable to find next stop time");

        var nextStopFormatted = timeConverter.MinuteOfDayToTimeOnly(nextStopTime)
            .ToString("HH:mm");

        return Ok(new NextStopResponse { NextStop = nextStopFormatted });
    }
}