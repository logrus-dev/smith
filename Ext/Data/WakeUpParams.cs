namespace Logrus.Smith.Ext.Data;

public record WakeUpParams(string InitialState, string? State, Dictionary<string, string> Params);
