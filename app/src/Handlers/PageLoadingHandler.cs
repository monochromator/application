using System;
using Chromely.CefGlue;
using Chromely.Core;
using Chromely.Core.Configuration;
using NLog;
using Xilium.CefGlue;

namespace Monochromator.App.Handlers {
    /// <summary>
    /// Handler for pages loading
    /// </summary>
    public class PageLoadingHandler : CefLoadHandler, IChromelyCustomHandler {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IChromelyConfiguration _config;
        private readonly IChromelyContainer _container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Configuration</param>
        /// <param name="container">Container</param>
        public PageLoadingHandler(IChromelyConfiguration config, IChromelyContainer container) {
            _config = config;
            _container = container;
        }

        protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode) {
            base.OnLoadEnd(browser, frame, httpStatusCode);
            Logger.Info($"{frame.Url} loaded");

            // Launch devTools
#if DEBUG
            if (Equals(frame.Url, _config.StartUrl + "#/")) {
                Logger.Debug("Open DevTools window");
                BrowserLauncher.Open(_config.Platform, _config.DevToolsUrl);
            }      
#endif
        }
    }
}