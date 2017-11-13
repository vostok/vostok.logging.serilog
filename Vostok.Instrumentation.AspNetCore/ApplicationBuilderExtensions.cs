﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Vostok.Commons.Extensions.UnitConvertions;
using Vostok.Metrics;

namespace Vostok.Instrumentation.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseVostok(this IApplicationBuilder app)
        {
            var configuration = app.ApplicationServices.GetService<IConfiguration>();
            var service = configuration.GetValue<string>("service");

            return app.UseMiddleware<RequestExecutionDistributedContextMiddleware>()
                .UseMiddleware<RequestExecutionTimeMiddleware>()
                .UseMiddleware<RequestExecutionTraceMiddleware>(service)
                .UseVostokLogging()
                .UseVostokSystemMetrics(1.Minutes());
        }

        public static IApplicationBuilder UseVostokLogging(this IApplicationBuilder app)
        {
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            applicationLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            return app;
        }

        public static IApplicationBuilder UseVostokSystemMetrics(this IApplicationBuilder app, TimeSpan period)
        {
            app.ApplicationServices.GetService<IMetricScope>().SystemMetrics(period);
            return app;
        }
    }
}