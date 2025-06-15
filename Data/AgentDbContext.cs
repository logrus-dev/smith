using Logrus.Smith.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Logrus.Smith.Data;

public class AgentDbContext: DbContext
{
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<Artifact> AgentOutcomes => Set<Artifact>();

    protected AgentDbContext()
    {
    }

    public AgentDbContext(DbContextOptions<AgentDbContext> options) : base(UpdateOptions(options))
    {

    }

    private static DbContextOptions<AgentDbContext> UpdateOptions(DbContextOptions<AgentDbContext> options)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AgentDbContext>();
        optionsBuilder.UseNpgsql(options.GetExtension<NpgsqlOptionsExtension>().ConnectionString, o =>
        {
            o.UseNodaTime();
            o.ConfigureDataSource(oo =>
            {
                oo.EnableDynamicJson();
            });
        }).UseSnakeCaseNamingConvention();

        return optionsBuilder.Options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>().Property(x => x.Params).HasColumnType("jsonb");
    }
}
