namespace Logrus.Smith.Ext.Data;

public enum Outcome
{
    /// <summary>
    /// First outcome in the series.
    /// </summary>
    Started,

    /// <summary>
    /// This outcome does not add anything to the problem solution.
    /// </summary>
    Unchanged,

    /// <summary>
    /// This outcome improves the solution, comparing to the previous outcome.
    /// </summary>
    Improved,

    /// <summary>
    /// This outcome makes solution worse, comparing to the previous outcome.
    /// </summary>
    Degraded,

    /// <summary>
    /// This outcome completes the solution and fully satisfies the requirements. No more outcomes should be produced.
    /// </summary>
    Completed,

    /// <summary>
    /// This outcome interrupts the series. No more outcomes should be produced.
    /// </summary>
    Interrupted
}
