using Logrus.Smith.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Logrus.Smith.Data;

public class AgentDbContext: DbContext
{
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<AgentOutcome> AgentOutcomes => Set<AgentOutcome>();

    protected AgentDbContext()
    {
    }

    public AgentDbContext(DbContextOptions<AgentDbContext> options) : base(UpdateOptions(options))
    {

    }

    private static DbContextOptions<AgentDbContext> UpdateOptions(DbContextOptions<AgentDbContext> options)
    {
        var cs = options.GetExtension<NpgsqlOptionsExtension>().ConnectionString;
        var optionsBuilder = new DbContextOptionsBuilder<AgentDbContext>();
        optionsBuilder.UseNpgsql(cs, o =>
        {
            o.UseNodaTime();
        }).UseSnakeCaseNamingConvention();

        return optionsBuilder.Options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>().Property(x => x.Params).HasColumnType("jsonb");
    }
}
