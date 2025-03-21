namespace Blog.Presentation;

public static class CorsConfigurationExtensions
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        var devFrontendUrl = configuration.GetSection("Cors:Development:FrontendUrl").Value
                             ?? throw new InvalidOperationException("Development FrontendUrl is missing in CORS configuration.");
        var prodFrontendUrl = configuration.GetSection("Cors:Production:FrontendUrl").Value
                              ?? throw new InvalidOperationException("Production FrontendUrl is missing in CORS configuration.");

        services.AddCors(options =>
        {
            options.AddPolicy("DevPolicy", policy =>
            {
                policy.WithOrigins(devFrontendUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
            });

            options.AddPolicy("ProdPolicy", policy =>
            {
                policy.WithOrigins(prodFrontendUrl)
                    .WithMethods("GET", "POST")
                    .AllowAnyHeader()
                    .SetIsOriginAllowedToAllowWildcardSubdomains();
            });
        });

        return services;
    }
}