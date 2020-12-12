using System;
using System.IO.Ports;
using Chromely.Core.Network;
using Monochromator.App.Mbed;
using NLog;

namespace Monochromator.App.Services.Mbed {
    // TODO: Doc
    public class MbedService {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private const uint PingResponse = 1234567;

        // TODO: Doc
        public string[] Controllers() => SerialPort.GetPortNames();

        // TODO: Doc
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

        // TODO: Doc
        public SerialConnection Connect(ChromelyRequest request) {
            // Check parameters
            if (!request.Parameters.ContainsKey("device")) {
                throw new ArgumentException("device isn't specified");
            }
            
            // Open connection
            var conn = new SerialConnection(request.Parameters["device"]);
            conn.Open();

            // Check connection
            if (!IsMonochromator(conn)) {
                throw new Exception(); // TODO: Change
            }

            return conn;
        }

        // TODO: Doc
        private static bool IsMonochromator(SerialConnection connection) {
            // Ping controller
            connection.Send((uint) PacketHeader.Ping);

            // Parse response
            var responseAsBytes = connection.Read(sizeof(PacketHeader));
            var response = BitConverter.ToUInt32(responseAsBytes);

            return response == PingResponse;
        }
    }
}