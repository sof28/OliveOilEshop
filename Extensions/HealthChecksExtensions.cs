using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace OliveOilEshop.Extensions
{
    public static class HealthChecksExtensions
    {
        public static IServiceCollection AddHealthChecksConfiguration(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<Database.ApplicationDbContext>();
            return services;
        }

        public static IEndpointRouteBuilder UseHealthChecks(this IEndpointRouteBuilder builder)
        {
            builder.MapHealthChecks("health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            return builder;
        }
    }
}
