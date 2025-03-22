using Blog.Application;
using Blog.Application.Behaviors;
using Blog.Application.Exeptions;
using Blog.Infrastructure;
using Blog.Infrastructure.Services;
using Blog.Presentation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://+:5000");
builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

Console.WriteLine($"Connection String: {builder.Configuration.GetConnectionString("PostgresConnection")}");

builder.Services.AddHttpClient<ElasticHttpClient>();
builder.Services.AddLogging();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMediatR(
    cfg =>
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

builder.Services.AddCustomCors(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PostgresContext>();
    int retries = 5;
    while (retries > 0)
    {
        try
        {
            dbContext.Database.CanConnect();
            Console.WriteLine("Successfully connected to PostgreSQL database!");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to connect to PostgreSQL database: {ex.Message}. Retries left: {retries}");
            retries--;
            if (retries == 0)
            {
                throw new Exception("Failed to connect to PostgreSQL after multiple attempts", ex);
            }
            Thread.Sleep(5000);
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseCors(app.Environment.IsDevelopment() ? "DevPolicy" : "ProdPolicy");
app.MapControllers();

app.Run();