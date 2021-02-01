using System;
using System.Threading;
using Monochromator.App.Exceptions;
using Monochromator.App.Mbed;
using Monochromator.App.Services.Mbed;

namespace Monochromator.App.Services.Calibration {
    /// <summary>
    /// Waiter for calibration
    /// </summary>
    public static class CalibrationWaiter {
        /// <summary>
        /// Wait calibration end
        /// </summary>
        /// <param name="controller">Controller connection1</param>
        /// <param name="token">Cancellation token</param>
        public static void Wait(SerialConnection controller, CancellationToken token) {
            while (true) {
                token.ThrowIfCancellationRequested();
                
                // Wait result flag
                try {
                    WaitEndFlag(controller);
                } catch (TimeoutException) {
                    // Catch timeout to wait infinitely
                    continue;
                }
                
                return;
            }
        }

        /// <summary>
        /// Wait calibration end
        /// </summary>
        /// <param name="controller">Controller connection</param>
        private static void WaitEndFlag(SerialConnection controller) {
            var response = (CalibratePacketHeader) BitConverter.ToUInt32(controller.Read(sizeof(CalibratePacketHeader)));
            switch (response) {
                case CalibratePacketHeader.Start:
                case CalibratePacketHeader.InvalidArguments:
                    throw new InvalidOperationException($"Invalid header received: {response}");
                
                case CalibratePacketHeader.End:
                    break;
                
                default:
                    throw new UnknownEnumValueException<CalibratePacketHeader>(response);
            }
        }
    }
}