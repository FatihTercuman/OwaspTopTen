using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Middlewares
{
    public static class ExceptionHandlingExtensions
    {
        public static IApplicationBuilder UseSecurityExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            return app;
        }
    }
}
