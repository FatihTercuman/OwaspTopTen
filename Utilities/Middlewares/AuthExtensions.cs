using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Middlewares
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddSecurityAuth(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {

            });

            services.AddAuthorization(options =>
            {

            });

            return services;
        }

        public static IApplicationBuilder UseSecurityAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
