using Microsoft.Extensions.DependencyInjection;

namespace Utilities.Middlewares
{
    public static class LoggingExtensions
    {
        public static IServiceCollection AddSecurityLogging(this IServiceCollection services)
        {
            services.AddLogging();
            return services;
        }
    }
}
