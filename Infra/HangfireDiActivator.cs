using Hangfire;

namespace Logrus.Smith.Infra;

public class HangfireDiActivator(IServiceProvider serviceProvider): JobActivator
{
    public override object? ActivateJob(Type type)
    {
        return serviceProvider.GetService(type);
    }
}
