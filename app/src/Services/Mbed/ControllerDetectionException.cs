using System;

namespace Monochromator.App.Services.Mbed {
    // TODO: Doc
    public class ControllerDetectionException : Exception {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controllers">Available controllers</param>
        public ControllerDetectionException(string[] controllers) : base(
            $"Unable to connect to one of the following controllers: {string.Join(", ", controllers)}") { }
    }
}