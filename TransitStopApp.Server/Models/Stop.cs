namespace TransitStopApp.Server.Models;

/// <summary>
/// Represents a stop along a transit route.
/// </summary>
public class Stop
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int StopOrder { get; set; }
}