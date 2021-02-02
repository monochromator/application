using Chromely.Core.Configuration;

namespace Monochromator.App {
    /// <summary>
    /// Class extensions
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// Notify an event to UI
        /// </summary>
        /// <param name="configuration">Configuration</param>
        /// <param name="event">Event's name</param>
        public static void NotifyEvent(this IChromelyConfiguration configuration, string @event) {
            configuration.JavaScriptExecutor.ExecuteScript(@$"window.dispatchEvent(new Event('{@event}'))");
        }
    }
}