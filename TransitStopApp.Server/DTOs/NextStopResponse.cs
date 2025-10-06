namespace TransitStopApp.Server.DTOs;

/// <summary>
/// DTO containing the GetNextStopTime result as a formatted string
/// </summary>
public class NextStopResponse
{
    public required string NextStop { get; set; }
}
