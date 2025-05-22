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

    [AutomaticRetry(Attempts = 0)]
    public async Task RunAgent(long id, CancellationToken ct, PerformContext context)
    {
        await using (await dlock.Acquire($"RunAgent/{id}"))
        {
            var db = getDb();
            var agent = await db.Agents.Include(x => x.Outcomes).FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception($"Agent {id} not found");
            bool completed;
            do
            {
                if (agent.InputTokensUsed >= agent.InputTokensLimit ||
                    agent.OutputTokensUsed >= agent.OutputTokensLimit)
                {
                    context.WriteLine(ConsoleTextColor.Yellow, "Token limit exceeded");
                    return;
                }

                context.WriteLine(ConsoleTextColor.Blue, "Waking up agent");
                var score = await WakeUpAgent(agent);
                context.WriteLine(ConsoleTextColor.Blue, "Agent has completed iteration with score {0}", score);
                completed = score == 100;
                await db.SaveChangesAsync();
            } while (!completed && !ct.IsCancellationRequested);

            if (ct.IsCancellationRequested)
            {
                Log.Information("Agent {AgentId} was cancelled", id);
                context.WriteLine(ConsoleTextColor.Yellow, "Agent was cancelled");
            }

            if (completed)
            {
                Log.Information("Agent {AgentId} has successfully completed", id);
                context.WriteLine(ConsoleTextColor.Green, "Agent has successfully completed");
            }
        }
    }

    private async Task<decimal> WakeUpAgent(Agent agent)
    {
        var handler = agentHandlers[agent.Key];
        var result = await handler.WakeUp(new WakeUpParams(agent.InitialState, agent.StateJson));
        agent.StateJson = result.StateJson;
        agent.InputTokensUsed += result.InputTokensUsed;
        agent.OutputTokensUsed += result.OutputTokensUsed;
        agent.Outcomes.Add(new AgentOutcome
        {
            CreatedAt = SystemClock.Instance.GetCurrentInstant(),
            Data = result.Outcome,
            Score = result.Score,
            Agent = agent
        });
        agent.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        return result.Score;
    }
}
