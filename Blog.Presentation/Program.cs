using Blog.Application;
using Blog.Application.Behaviors;
using Blog.Application.Exeptions;
using Blog.Infrastructure;
using Blog.Infrastructure.Services;
using Blog.Presentation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseCors(app.Environment.IsDevelopment() ? "DevPolicy" : "ProdPolicy");
app.MapControllers();

app.Run();