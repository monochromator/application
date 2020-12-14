using System;
using Chromely.Core.Network;

namespace Monochromator.App.Controllers {
    /// <summary>
    /// Chromely response defining an error
    /// </summary>
    public class ErrorResponse : ChromelyResponse {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="e">Exception</param>
        public ErrorResponse(Exception e) {
            StatusText = "KO";
            Data = e.Message;
            Status = 503;
        }
    }
}