using NodaTime;

namespace Logrus.Smith.Data.Entities;

public class Agent
{
    public long Id { get; init; }
    public required string Key { get; set; }
    public required string Name { get; init; }
    public required int InputTokensLimit { get; init; }
    public required int OutputTokensLimit { get; init; }
    public int InputTokensUsed { get; set; }
    public int OutputTokensUsed { get; set; }
    public string? StateJson { get; set; }
    public required string InitialState { get; set; }
    public required Instant CreatedAt { get; init; }
    public required Instant UpdatedAt { get; set; }
    public required ICollection<AgentOutcome> Outcomes { get; init; }
}
