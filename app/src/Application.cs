using System;
using System.Threading;
using Chromely;
using Chromely.Core;
using Chromely.Core.Configuration;
using Monochromator.App.Handlers;
using NLog;

namespace Monochromator.App {
    /// <summary>
    /// Chromely application
    /// </summary>
    public class Application : ChromelyBasicApp {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public override void Configure(IChromelyContainer container) {
            base.Configure(container);

            // Register handlers
            container.RegisterSingleton<IChromelyCustomHandler, PageLoadingHandler>(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Run backend code
        /// </summary>
        /// <param name="config">Configuration</param>
        /// <param name="container">Container</param>
        public static void Run(IChromelyConfiguration config, IChromelyContainer container) {
            Logger.Info("Start main thread");

            // Check JS executor
            var jsExecutor = config.JavaScriptExecutor ?? throw new Exception("Communication with UI is impossible");
            Logger.Info($"Communication with UI enabled");

            // TODO: Write code
            while (true) {
                Thread.Yield();
            }
        }
    }
}