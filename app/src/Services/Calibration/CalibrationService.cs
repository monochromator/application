using System;
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

namespace Monochromator.App.Services.Calibration {
    /// <summary>
    /// Service for calibration
    /// </summary>
    public class CalibrationService {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IChromelyConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public CalibrationService(IChromelyConfiguration configuration) {
            _configuration = configuration;
        }

        /// <summary>
        /// Launch calibration
        /// </summary>
        /// <param name="request">Calibration request</param>
        /// <returns>Calibration cancellation token</returns>
        public CancellationTokenSource Calibrate(ChromelyRequest request) {
            var arguments = ParseCalibrationRequest(request);

            // Get controller
            var controller = MbedService.Connection ?? throw new Exception("No controller connected");

            // Send arguments
            controller.Send((uint) PacketHeader.Calibrate);
            controller.Send(arguments.Wavelength);

            // Wait start
            var response = (CalibratePacketHeader) BitConverter.ToUInt32(controller.Read(sizeof(CalibratePacketHeader)));
            switch (response) {
                case CalibratePacketHeader.Start:
                    break;

                case CalibratePacketHeader.End:
                    throw new InvalidOperationException("End is sent before start");

                case CalibratePacketHeader.InvalidArguments:
                    throw new Exception($"Invalid calibration arguments (wavelength: {arguments.Wavelength})");
                
                default:
                    throw new UnknownEnumValueException<CalibratePacketHeader>(response);
            }

            Logger.Info($"Start calibration with arguments (wavelength: {arguments.Wavelength})");

            // Wait results in background
            var cancelTokenSource = new CancellationTokenSource();
            Task.Run(() => {
                try {
                    // Notify end
                    CalibrationWaiter.Wait(controller, cancelTokenSource.Token);
                    NotifyCalibrationEnd();
                } catch (OperationCanceledException) {
                    Logger.Info("Calibration waiter cancelled");
                } catch (Exception e) {
                    Logger.Error(e);
                    NotifyCalibrationError();
                }
            }, cancelTokenSource.Token);

            return cancelTokenSource;
        }
        
        /// <summary>
        /// Parse calibration arguments in the given request
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Arguments</returns>
        private static CalibrationArguments ParseCalibrationRequest(ChromelyRequest request) {
            // Parse postData
            var arguments = JsonSerializer.Deserialize<CalibrationArguments>(request.PostData?.ToString() ?? "{}");

            // Check value
            if (float.IsNaN(arguments.Wavelength)) {
                throw new Exception("Calibration parameters contain NaN numbers");
            }

            // Assert wavelength
            if (arguments.Wavelength <= 0) {
                throw new Exception($"Invalid calibration wavelength: {arguments.Wavelength}");
            }

            return arguments;
        }

        /// <summary>
        /// Notify UI that calibration failed
        /// </summary>
        private void NotifyCalibrationError() {
            _configuration.NotifyEvent("calibration.error");
        }

        /// <summary>
        /// Notify UI that calibration ended
        /// </summary>
        private void NotifyCalibrationEnd() {
            _configuration.NotifyEvent("calibration.end");
        }
    }
}