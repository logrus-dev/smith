using Logrus.Smith.Ext.Data;

namespace Logrus.Smith.Ext;

public interface IAgent
{
    Task<WakeUpResult> WakeUp(WakeUpParams param);
}
