namespace Logrus.Smith.Settings;

public class LogrusSmithSettings
{
    public required string DbConnectionString { get; init; }
    public required string OpenAiEndpoint { get; init; }
    public required string OpenAiApiKey { get; init; }
    public required string OpenAiModel { get; init; }
    public required bool UseHighReasoningEffort { get; init; }
    public required string AgentEndpoint { get; init; }
    public required string BacktestEndpoint { get; init; }
}
