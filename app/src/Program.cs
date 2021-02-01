using System;
using Chromely.Core;
using Chromely.Core.Configuration;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Monochromator.App {
    /// <summary>
    /// Entry point
    /// </summary>
    internal static class Program {
        [STAThread]
        private static void Main(string[] args) {
            // Configure logger
            ConfigureLogger();

            // Run application
            AppBuilder
                .Create()
                .UseApp<Application>()
                .UseConfig<IChromelyConfiguration>(Configuration.Create())
                .Build()
                .Run(args);
        }

        /// <summary>
        /// Configure logger
        /// </summary>
        private static void ConfigureLogger() {
            var config = new LoggingConfiguration();

            // Define layout
            const string? layout = "${longdate} [Thread-${threadid}] [${level}] ${logger} - ${message}";

            // Define target
            var console = new ConsoleTarget {
                Layout = layout
            };
            var file = new FileTarget {
                Layout = layout,
                FileName = "${basedir}/Logs/application.log",

                ArchiveFileName = "${basedir}/Logs/application_{#}.log",
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveAboveSize = 1 * 1000 * 1000, // 1Mo
                MaxArchiveFiles = 3,
            };

            // Add rules
#if DEBUG
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, console);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, file);
#else
            config.AddRule(LogLevel.Info, LogLevel.Fatal, console);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, file);
#endif

            // Apply
            LogManager.Configuration = config;
        }
    }
}