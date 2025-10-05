namespace TransitStopApp.Server.Models;

/// <summary>
/// Represents a scheduled stop time for a transit stop.
/// </summary>
public class StopTime
{
    public int Id { get; set; }
    public int StopId { get; set; }
    public int StopMinuteOfDay { get; set; }
}
