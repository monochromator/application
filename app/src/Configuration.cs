using Chromely.Core.Configuration;

namespace Monochromator.App {
    /// <summary>
    /// Custom configuration for chromely
    /// </summary>
    public static class Configuration {
        /// <summary>
        /// Create Chromely configuration
        /// </summary>
        /// <returns>Configuration</returns>
        public static IChromelyConfiguration Create() {
            var config = DefaultConfiguration.CreateForRuntimePlatform();

            // Update configuration
            config.AppName = "monochromator";
#if DEBUG
            config.StartUrl = "http://localhost:8080/";
#else
            config.StartUrl = "local://ui/index.html";
#endif
#if DEBUG
            config.DebuggingMode = true;
#else
            config.DebuggingMode = false;
#endif
            config.CefDownloadOptions = new CefDownloadOptions {
                AutoDownloadWhenMissing = false,
                DownloadSilently = false
            };
            config.WindowOptions.RelativePathToIconFile = "icon.ico";

            return config;
        }
    }
}