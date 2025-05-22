using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Logrus.Smith.Data;

public class DesignTimeDataContextFactory: IDesignTimeDbContextFactory<AgentDbContext>
{
    public AgentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AgentDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost; User Id=postgres;Password=1", o =>
        {
            o.UseNodaTime();
        }).UseSnakeCaseNamingConvention();
        return new AgentDbContext(optionsBuilder.Options);
    }
}
