using System;
using System.IO.Ports;
using Chromely.Core;
using Chromely.Core.Network;
using Monochromator.App.Exceptions;
using Monochromator.App.Mbed;
using NLog;

namespace Monochromator.App.Services.Mbed {
    /// <summary>
    /// Service for MBED
    /// </summary>
    public class MbedService {
        private const uint PingResponse = 1234567;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IChromelyContainer _container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Container</param>
        public MbedService(IChromelyContainer container) {
            _container = container;
        }

        /// <summary>
        /// Get connected devices' names
        /// </summary>
        /// <returns>Devices</returns>
        public string[] Controllers() => SerialPort.GetPortNames();

        /// <summary>
        /// Autodetect monochromator controller in connected devices
        /// </summary>
        /// <returns>Controller's device name</returns>
        /// <exception cref="ControllerDetectionException"></exception>
        public string Autodetect() {
            // Get connected controllers
            var controllers = Controllers();

            foreach (var controller in controllers) {
                try {
                    // Open connection
                    using var conn = new SerialConnection(controller, 1000);
                    conn.Open();

                    // Check connection
                    if (IsMonochromator(conn)) {
                        Logger.Info($"Autodection has chosen: {controller}");
                        return controller;
                    }
                } catch (Exception e) {
                    Logger.Error(e);
                }
            }

            throw new ControllerDetectionException(controllers);
        }

        /// <summary>
        /// Try to connect to the given device
        /// </summary>
        /// <param name="request">Connection request</param>
        /// <returns>Connection</returns>
        public SerialConnection Connect(ChromelyRequest request) {
            // Check parameters
            if (!request.Parameters.ContainsKey("device")) {
                throw new ArgumentException("device isn't specified");
            }

            // Open connection
            var device = request.Parameters["device"];
            var conn = new SerialConnection(device);
            conn.Open();

            // Check connection
            if (!IsMonochromator(conn)) {
                throw new Exception($"{device} isn't a monochromator controller");
            }

            return conn;
        }

        /// <summary>
        /// Test whether the given connection is a link with monochromator controller
        /// </summary>
        /// <param name="connection">Connection</param>
        /// <returns>true if connection is a link with monochromator controller, false otherwise</returns>
        private static bool IsMonochromator(SerialConnection connection) {
            // Ping controller
            connection.Send((uint) PacketHeader.Ping);

            // Parse response
            var responseAsBytes = connection.Read(sizeof(PacketHeader));
            var response = BitConverter.ToUInt32(responseAsBytes);

            return response == PingResponse;
        }

        /// <summary>
        /// Ping connected controller and throw an exception if unable to ping it
        /// </summary>
        public void Ping() {
            // Get controller
            var controller = _container.GetInstance<SerialConnection>(typeof(SerialConnection).FullName) ??
                             throw new Exception("No controller connected");

            if (!IsMonochromator(controller)) {
                throw new Exception("Unable to ping connected device");
            }
        }
    }
}