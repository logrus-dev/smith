using Logrus.Smith.Ext.Data;
using NodaTime;

namespace Logrus.Smith.Data.Entities;

public class Artifact
{
    public long Id { get; init; }
    public required string Data { get; set; }
    public required string Message { get; set; }
    public required decimal Score { get; set; }
    public required Outcome Outcome { get; init; }
    public required Instant CreatedAt { get; init; }
    public required Agent Agent { get; init; }
}
