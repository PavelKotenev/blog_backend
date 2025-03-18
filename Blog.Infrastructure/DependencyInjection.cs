using Blog.Contracts.Interfaces.Repositories;
using Blog.Infrastructure.Repositories.Posts.Commands;
using Blog.Infrastructure.Repositories.Posts.Queries;
using Blog.Infrastructure.Repositories.Tags.Commands;
using Blog.Infrastructure.Repositories.Tags.Queries;
using Blog.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Blog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var pgConnectionString = configuration.GetConnectionString("PostgresConnection");

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(pgConnectionString);

        dataSourceBuilder.EnableDynamicJson();

        var dataSource = dataSourceBuilder.Build();

        services.AddSingleton(dataSource)
            .AddSingleton<ElasticHttpClient>()
            .AddScoped<IPostRepositories.IElasticQuery, ElasticPostQueryRepository>()
            .AddScoped<IPostRepositories.IElasticCommand, ElasticPostCommandRepository>()
            .AddScoped<IPostRepositories.IPostgresCommand, PostgresPostCommandRepository>()
            .AddScoped<IPostRepositories.IPostgresQuery, PostgresPostQueryRepository>()
            .AddScoped<ITagRepositories.IPostgresCommand, PostgresTagCommandRepository>()
            .AddScoped<ITagRepositories.IPostgresQuery, PostgresTagQueryRepository>()
            .AddScoped<ElasticResponseMapper>()
            .AddScoped<FakerService>();

        return services;
    }
}