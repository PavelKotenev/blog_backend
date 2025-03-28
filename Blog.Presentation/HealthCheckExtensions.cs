using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Blog.Presentation;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddCustomHealthChecks(
        this IServiceCollection services,
        string postgresConnection,
        string elasticSearchUrl
    )
    {
        services.AddHealthChecks()
            .AddNpgSql(
                postgresConnection,
                name: "PostgreSQL",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "postgres" })
            .AddElasticsearch(
                elasticSearchUrl,
                name: "ElasticSearch",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "search", "elasticsearch" });

        return services;
    }

    public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });

        return app;
    }
}