using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Blog.Infrastructure;

public class PostgresContextFactory : IDesignTimeDbContextFactory<PostgresContext>
{
    public PostgresContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Blog.Presentation"))
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));

        return new PostgresContext(optionsBuilder.Options);
    }
}