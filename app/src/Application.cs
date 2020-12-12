using System;
using Chromely;
using Chromely.Core;
using Chromely.Core.Network;
using Monochromator.App.Controllers;

namespace Monochromator.App {
    /// <summary>
    /// Chromely application
    /// </summary>
    public class Application : ChromelyBasicApp {
        public override void Configure(IChromelyContainer container) {
            base.Configure(container);

            // Register controllers
            container.RegisterSingleton<ChromelyController, MbedController>(Guid.NewGuid().ToString());
        }
    }
}