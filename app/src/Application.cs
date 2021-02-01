using System;
using Chromely;
using Chromely.Core;
using Chromely.Core.Network;
using Microsoft.Extensions.DependencyInjection;
using Monochromator.App.Controllers;

namespace Monochromator.App {
    /// <summary>
    /// Chromely application
    /// </summary>
    public class Application : ChromelyBasicApp {
        public override void ConfigureServices(IServiceCollection services) {
            base.ConfigureServices(services);
            
            // Register controllers
            services.AddSingleton<ChromelyController, MbedController>();
            services.AddSingleton<ChromelyController, AnalysisController>();
            services.AddSingleton<ChromelyController, CalibrationController>();
        }
    }
}