using System;
using System.Threading;
using Monochromator.App.Exceptions;
using Monochromator.App.Mbed;
using Monochromator.App.Services.Mbed;
using NLog;

namespace Monochromator.App.Services.Analysis {
    /// <summary>
    /// Waiter for analysis
    /// </summary>
    public static class AnalysisResultsWaiter {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Wait analysis results
        /// </summary>
        /// <param name="controller">Controller connection</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Results</returns>
        public static (float, float)[] Wait(SerialConnection controller, CancellationToken token) {
            while (true) {
                token.ThrowIfCancellationRequested();

                // Wait result flag
                try {
                    WaitResultFlag(controller);
                } catch (TimeoutException) {
                    // Catch timeout to wait infinitely
                    continue;
                }

                // Read results
                var results = new (float, float)[BitConverter.ToUInt32(controller.Read(sizeof(uint)))];
                for (var i = 0; i < results.Length; i++) {
                    var wavelength = BitConverter.ToSingle(controller.Read(sizeof(float)));
                    var value = BitConverter.ToSingle(controller.Read(sizeof(float)));

                    results[i] = (wavelength, value);
                }

                Logger.Debug($"Analysis results received: {AnalysisService.ToJson(results)}");
                return results;
            }
        }

        /// <summary>
        /// Wait result flag
        /// </summary>
        /// <param name="controller">Controller connection</param>
        private static void WaitResultFlag(SerialConnection controller) {
            var response = (AnalysisPacketHeader) BitConverter.ToUInt32(controller.Read(sizeof(AnalysisPacketHeader)));
            switch (response) {
                case AnalysisPacketHeader.InvalidArguments:
                case AnalysisPacketHeader.Start:
                    throw new InvalidOperationException($"Invalid header received: {response}");

                case AnalysisPacketHeader.ResultsSize:
                    break;

                default:
                    throw new UnknownEnumValueException<AnalysisPacketHeader>(response);
            }
        }
    }
}