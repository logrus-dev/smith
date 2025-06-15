using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Logrus.Ext.Api;
using Logrus.Smith.Data;
using Logrus.Smith.Data.Entities;
using Logrus.Smith.Ext;
using Logrus.Smith.Ext.Data;
using Logrus.Smith.Infra;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Serilog;

namespace Logrus.Smith;

public class AgentHypervisor(Func<AgentDbContext> getDb, IPluginCollection<IAgent> agentHandlers, DistributedLockFactory dlock)
{
    public void Register()
    {
        RecurringJob.AddOrUpdate("DiscoverAgents", () => DiscoverAgents(default!), Cron.Never);
    }

    public async Task DiscoverAgents(PerformContext context)
    {
        var db = getDb();
        var agents = await db.Agents.ToArrayAsync();
        foreach (var agent in agents)
        {
            RecurringJob.AddOrUpdate($"Agent {agent.Name} ({agent.Key})", () => RunAgent(agent.Id, default, default!), Cron.Never);
        }
        context.WriteLine(ConsoleTextColor.Blue, "Discovered {0} agents", agents.Length);
    }

    [AutomaticRetry(Attempts = 1, DelaysInSeconds = [10])]
    public async Task RunAgent(long id, CancellationToken ct, PerformContext context)
    {
        await using (await dlock.Acquire($"RunAgent/{id}"))
        {
            WakeUpResult wakeUpResult;
            do
            {
                var db = getDb();
                var agent = await db.Agents.Include(x => x.Artifacts).FirstOrDefaultAsync(x => x.Id == id)
                    ?? throw new Exception($"Agent {id} not found");
                if (!agent.IsActive)
                {
                    Log.Information("Agent {AgentId} is disabled", id);
                    context.WriteLine(ConsoleTextColor.Yellow, "Agent is disabled");
                    return;
                }
                context.WriteLine(ConsoleTextColor.Blue, "Waking up agent");
                wakeUpResult = await WakeUpAgent(agent);
                if (wakeUpResult.Outcome == Outcome.Interrupted)
                {
                    context.WriteLine(ConsoleTextColor.Yellow, "Agent interrupted: {0}", wakeUpResult.Message);
                    return;
                }
                context.WriteLine(ConsoleTextColor.Blue, "Agent has produced an outcome {0}: {1}", wakeUpResult.Outcome, wakeUpResult.Message);

                await db.SaveChangesAsync();
            } while (wakeUpResult.Outcome != Outcome.Completed && !ct.IsCancellationRequested);

            if (ct.IsCancellationRequested)
            {
                Log.Information("Agent {AgentId} was cancelled", id);
                context.WriteLine(ConsoleTextColor.Yellow, "Agent was cancelled");
            }

            if (wakeUpResult.Outcome == Outcome.Completed)
            {
                Log.Information("Agent {AgentId} has successfully completed", id);
                context.WriteLine(ConsoleTextColor.Green, "Agent has successfully completed");
            }
        }
    }

    private async Task<WakeUpResult> WakeUpAgent(Agent agent)
    {
        var handler = agentHandlers[agent.Key];
        var result = await handler.WakeUp(new WakeUpParams(agent.InitialState, agent.State, agent.Params));
        agent.State = result.State;
        agent.Artifacts.Add(new Artifact
        {
            CreatedAt = SystemClock.Instance.GetCurrentInstant(),
            Data = result.Data,
            Outcome = result.Outcome,
            Agent = agent,
            Message = result.Message,
            Score = result.Score,
        });
        agent.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        return result;
    }
}
