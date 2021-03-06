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
            Func<IAirlockClient> getAirlockClient, 
            Func<string> getRoutingKey,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));

            var sink = new AirlockSink(getAirlockClient, getRoutingKey);
            return loggerConfiguration.Sink(sink, restrictedToMinimumLevel);
        }

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

        public static LoggerConfiguration VostokLog(
            this LoggerSinkConfiguration loggerConfiguration,
            ILog log,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));

            var sink = new VostokLogSink(log);
            return loggerConfiguration.Sink(sink, restrictedToMinimumLevel);
        }
    }
}