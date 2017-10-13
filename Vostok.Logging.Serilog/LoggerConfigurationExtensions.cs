﻿using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Vostok.Airlock;
using Vostok.Logging.Serilog.Sinks;

namespace Vostok.Logging.Serilog
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration Airlock(
            this LoggerSinkConfiguration loggerConfiguration,
            IAirlockClient airlockClient,
            string routingKey,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));

            var sink = new AirlockSink(airlockClient, routingKey);
            return loggerConfiguration.Sink(sink, restrictedToMinimumLevel);
        }
    }
}