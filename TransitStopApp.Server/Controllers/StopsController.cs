using Microsoft.AspNetCore.Mvc;
using TransitStopApp.Server.Interfaces;
using TransitStopApp.Server.Models;

namespace TransitStopApp.Server.Controllers;

/// <summary>
/// API endpoints for stop data and their next scheduled stop times.
/// </summary>
/// <param name="stopOperations"></param>
/// <param name="timeConverter"></param>
/// <param name="currentTimeFetcher"></param>
[ApiController]
[Route("api/[controller]")]
public class StopsController(ITransitStopOperations stopOperations,
    IMinuteOfDayConverter timeConverter, ICurrentTimeFetcher currentTimeFetcher)
    : ControllerBase
{
    /// <summary>
    /// Fetch all Stops ordered by StopOrder
    /// </summary>
    /// <returns>An IEnumerable of all the stops</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Stop>>> Get()
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
    public async Task<ActionResult<string>> GetStop(int stopId)
    {
        var currentTime = currentTimeFetcher.Fetch();
        var minuteOfDay = timeConverter.TimeOnlyToMinuteOfDay(currentTime);
        var nextStopTime = await stopOperations
            .GetNextStopTimeAsync(stopId, minuteOfDay);

        if (nextStopTime < 0)
            return NotFound("Unable to find next stop time");

        var nextStopStr = timeConverter.MinuteOfDayToTimeOnly(nextStopTime)
            .ToString("HH:mm");

        return Ok(new { nextStop = nextStopStr });
    }
}