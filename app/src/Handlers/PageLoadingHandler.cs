using System;
using System.Threading.Tasks;
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Configuration</param>
        public PageLoadingHandler(IChromelyConfiguration config) => _config = config;
        
        protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode) {
            base.OnLoadEnd(browser, frame, httpStatusCode);
            Logger.Info($"{frame.Url} loaded");
            
            // Start main thread
            if (Equals(frame.Url, _config.StartUrl)) {
                Task.Factory.StartNew(() => {
                    try {
                        Application.Run(_config);
                    } catch (Exception e) {
                        Logger.Fatal($"{e.Message}\n{e.StackTrace}");
                        
                        // Kill browser
                        browser.GetHost().CloseBrowser(true);
                    }
                }, TaskCreationOptions.LongRunning);
            }
        }
    }
}