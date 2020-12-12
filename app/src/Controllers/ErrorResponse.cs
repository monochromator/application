using System;
using Chromely.Core.Network;

namespace Monochromator.App.Controllers {
    // TODO: Doc
    public class ErrorResponse : ChromelyResponse {
        // TODO: Doc
        public ErrorResponse(Exception e) {
            StatusText = "KO";
            Data = e.Message;
            Status = 503;
        }
    }
}