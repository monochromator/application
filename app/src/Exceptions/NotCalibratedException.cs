using System;

namespace Monochromator.App.Exceptions {
    /// <summary>
    /// Exception thrown when trying to launch an analysis without calibrating the controller
    /// </summary>
    public class NotCalibratedException : Exception {
        public const string Code = "NOT_CALIBRATED";

        /// <summary>
        /// Constructor
        /// </summary>
        public NotCalibratedException() : base("Controller isn't calibrated") { }
    }
}