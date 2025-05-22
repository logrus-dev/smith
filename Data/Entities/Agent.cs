using NodaTime;

namespace Logrus.Smith.Data.Entities;

public class Agent
{
    public long Id { get; init; }
    public required string Key { get; set; }
    public required string Name { get; init; }
    public string? State { get; set; }
    public required string InitialState { get; set; }
    public required Dictionary<string, string> Params { get; set; }
    public required Instant CreatedAt { get; init; }
    public required Instant UpdatedAt { get; set; }
    public required ICollection<AgentOutcome> Outcomes { get; init; }
}
