using Blog.Application;
using Blog.Application.Behaviors;
using Blog.Application.Exeptions;
using Blog.Infrastructure;
using Blog.Infrastructure.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blog.Presentation;

public class Program
{
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        var configuration = builder.Configuration;

        Console.WriteLine(builder.Environment.EnvironmentName);
        
        builder.Services.AddDbContext<PostgresContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));

        builder.Services.AddHttpClient<ElasticHttpClient>();

        builder.Services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
            logging.SetMinimumLevel(Enum.Parse<LogLevel>("Information"));
        });

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationMarker).Assembly);
            cfg.AddOpenBehavior(typeof(RequestResponseLoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddValidatorsFromAssemblyContaining<ApplicationMarker>();
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCustomCors(configuration.GetValue<string>("Cors:FrontendUrl"));
        builder.Services.AddCustomHealthChecks(
            configuration.GetConnectionString("PostgresConnection"),
            configuration.GetValue<string>("ElasticSearch:Url")
        );

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseCustomHealthChecks();
        app.UseRouting();
        app.MapControllers();
        app.UseCors(builder.Environment.EnvironmentName);
        app.Run();
    }
}