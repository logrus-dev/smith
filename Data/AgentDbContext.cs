using Logrus.Smith.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logrus.Smith.Data;

public class AgentDbContext: DbContext
{
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<AgentOutcome> AgentOutcomes => Set<AgentOutcome>();

    protected AgentDbContext()
    {
    }

    public AgentDbContext(DbContextOptions<AgentDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>().Property(x => x.Params).HasColumnType("jsonb");
    }
}
