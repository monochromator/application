using System;

namespace Monochromator.App.Services.Mbed {
    /// <summary>
    /// Exception thrown when <see cref="MbedService"/> can't detect a monochromator controller
    /// </summary>
    public class ControllerDetectionException : Exception {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controllers">Available controllers</param>
        public ControllerDetectionException(string[] controllers) : base(
            $"Unable to connect to one of the following controllers: {string.Join(", ", controllers)}") { }
    }
}