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
        /// <param name="text">Status text</param>
        public ErrorResponse(Exception e, string? text = null) {
            StatusText = text ?? "KO";
            Data = e.Message;
            Status = 503;
        }
    }
}