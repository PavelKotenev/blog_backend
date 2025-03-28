namespace Blog.Presentation;

public static class CorsConfigurationExtensions
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services, string frontendUrl)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("Development", policy =>
            {
                policy.WithOrigins(frontendUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
            });

            options.AddPolicy("Production", policy =>
            {
                policy.WithOrigins(frontendUrl)
                    .WithMethods("GET", "POST")
                    .AllowAnyHeader()
                    .SetIsOriginAllowedToAllowWildcardSubdomains();
            });
        });

        return services;
    }
}