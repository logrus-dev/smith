using Logrus.Smith.Settings;
using Medallion.Threading.Postgres;

namespace Logrus.Smith.Infra;

public class DistributedLockFactory(LogrusSmithSettings settings)
{
    public async Task<IAsyncDisposable> Acquire(string name)
    {
        var @lock = new PostgresDistributedLock(new PostgresAdvisoryLockKey(name, allowHashing: true), settings.DbConnectionString);
        return await @lock.AcquireAsync();
    }
}
