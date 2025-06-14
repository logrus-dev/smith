using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Logrus.Ext;
using Logrus.Smith.Data;
using Logrus.Smith.Infra;
using Logrus.Smith.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logrus.Smith;

public class Module: IModule
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        var dataSettings = services.AddSettings<LogrusSmithSettings>(configuration);
        services.AddDbContext<AgentDbContext>(options =>
        {
            options.UseNpgsql(dataSettings.DbConnectionString);
        }, ServiceLifetime.Transient);
        services.AddSingleton<Func<AgentDbContext>>(sp => sp.GetRequiredService<AgentDbContext>);
        services.AddTransient<AgentHypervisor>();
        services.AddSingleton<AgentCallbackManager>();

        services.AddHangfireServer();
        services.AddHangfire(config =>
        {
            config.UsePostgreSqlStorage(c => c.UseNpgsqlConnection(dataSettings.DbConnectionString), new PostgreSqlStorageOptions()
            {
                InvisibilityTimeout = TimeSpan.FromHours(24),
            });
            config.UseConsole();
        });
        services.AddSingleton<DistributedLockFactory>();
    }

    public async Task RunServices(IServiceProvider services)
    {
        GlobalConfiguration.Configuration.UseActivator(new HangfireDiActivator(services));
        var db = services.GetRequiredService<AgentDbContext>();
        await db.Database.MigrateAsync();
        var hypervisor = services.GetRequiredService<AgentHypervisor>();
        hypervisor.Register();
    }
}
