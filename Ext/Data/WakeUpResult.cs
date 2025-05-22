namespace Logrus.Smith.Ext.Data;

/// <summary>
/// Scoring pattern:
/// 0: Outcome causing errors and not working properly
/// 1 - 50: Outcome is not acceptable and has to be improved
/// 50 - 99: Outcome is acceptable but can be improved even more
/// 100: Outcome achieves the goal and does not need improvements
/// </summary>
/// <param name="StateJson"></param>
/// <param name="InputTokensUsed"></param>
/// <param name="OutputTokensUsed"></param>
/// <param name="Outcome"></param>
/// <param name="Score"></param>
public record WakeUpResult(string StateJson, int InputTokensUsed, int OutputTokensUsed, string Outcome, decimal Score);
