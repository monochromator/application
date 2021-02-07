using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Network;
using Monochromator.App.Exceptions;
using Monochromator.App.Mbed;
using Monochromator.App.Services.Mbed;
using NLog;

namespace Monochromator.App.Services.Analysis {
    /// <summary>
    /// Service for analysis
    /// </summary>
    public class AnalysisService {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IChromelyConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public AnalysisService(IChromelyConfiguration configuration) {
            _configuration = configuration;
        }

        /// <summary>
        /// Run analysis
        /// </summary>
        /// <param name="request">Analysis request</param>
        /// <returns>Analysis cancellation token</returns>
        public CancellationTokenSource Analyse(ChromelyRequest request) {
            // Parse arguments
            var arguments = ParseAnalysisRequest(request);

            // Get controller
            var controller = MbedService.Connection ?? throw new Exception("No controller connected");

            // Send arguments
            controller.Send((uint) PacketHeader.Analysis);
            controller.Send(arguments.Start);
            controller.Send(arguments.End);
            controller.Send(arguments.Step);

            // Wait start
            var response = (AnalysisPacketHeader) BitConverter.ToUInt32(controller.Read(sizeof(AnalysisPacketHeader)));
            switch (response) {
                case AnalysisPacketHeader.InvalidArguments:
                    throw new Exception($"Invalid analysis arguments (start: {arguments.Start}, end: {arguments.End}, step: {arguments.Step})");
                
                case AnalysisPacketHeader.NotCalibrated:
                    throw new NotCalibratedException();

                case AnalysisPacketHeader.Start:
                    break;

                case AnalysisPacketHeader.ResultsSize:
                    throw new InvalidOperationException("Results are sent after start");

                default:
                    throw new UnknownEnumValueException<AnalysisPacketHeader>(response);
            }

            Logger.Info($"Start analysis with arguments (start: {arguments.Start}, end: {arguments.End}, step: {arguments.Step})");

            // Wait results in background
            var cancelTokenSource = new CancellationTokenSource();
            Task.Run(() => {
                try {
                    // Send results
                    SendAnalysisResults(AnalysisResultsWaiter.Wait(controller, cancelTokenSource.Token), arguments.Name);
                } catch (OperationCanceledException) {
                    Logger.Info("Analysis results waiter cancelled");
                } catch (Exception e) {
                    Logger.Error(e);
                    SendAnalysisError();
                }
            }, cancelTokenSource.Token);

            return cancelTokenSource;
        }

        /// <summary>
        /// Parse analysis arguments in the given request
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Arguments</returns>
        private static AnalysisArguments ParseAnalysisRequest(ChromelyRequest request) {
            // Parse postData
            var arguments = JsonSerializer.Deserialize<AnalysisArguments>(request.PostData?.ToString() ?? "{}");

            // Check value
            if (float.IsNaN(arguments.Start) || float.IsNaN(arguments.End) || float.IsNaN(arguments.Step)) {
                throw new Exception("Analysis parameters contain NaN numbers");
            }

            // Assert range
            if (arguments.Start > arguments.End) {
                throw new Exception($"Invalid analysis range from {arguments.Start} to {arguments.End}");
            }

            // Assert step
            if (arguments.Step <= 0) {
                throw new Exception($"Invalid analysis step: {arguments.Step}");
            }

            return arguments;
        }

        /// <summary>
        /// Send results to UI
        /// </summary>
        /// <param name="results">Results to send</param>
        /// <param name="name">Analysis' name</param>
        private void SendAnalysisResults(IEnumerable<(float, float)> results, string? name) {
            _configuration.JavaScriptExecutor.ExecuteScript(
                $@"window.dispatchEvent(new CustomEvent('analysis.end', {{ detail: {{ data: [{ToJson(results)}], name: {(name is null ? "undefined" : $"\"{name}\"")} }} }}))");
        }

        /// <summary>
        /// Send analysis error to UI
        /// </summary>
        private void SendAnalysisError() {
            _configuration.NotifyEvent("analysis.error");
        }

        /// <summary>
        /// Convert the given results into JSON array
        /// </summary>
        /// <param name="results">Results</param>
        /// <returns>JSON array</returns>
        public static string ToJson(IEnumerable<(float, float)> results) {
            NumberFormatInfo nbInfo = new NumberFormatInfo {
                NumberDecimalSeparator = "."
            };

            return string.Join(",", results.Select(pair => $"[{pair.Item1.ToString(nbInfo)},{pair.Item2.ToString(nbInfo)}]"));
        }
    }
}