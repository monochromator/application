using System;
using System.IO.Ports;

namespace Monochromator.App.Mbed {
    /// <summary>
    /// Managed serial connection with MBED
    /// </summary>
    public class SerialConnection : IDisposable {
        private readonly SerialPort _port;
        private bool _disposed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="port">Port name</param>
        /// <param name="timeout">Request timeout</param>
        public SerialConnection(string port, int timeout = 5000) {
            _port = new SerialPort {
                ReadTimeout = timeout,
                WriteTimeout = timeout,
                PortName = port,
                BaudRate = 9600,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One
            };
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~SerialConnection() {
            Dispose();
        }

        /// <summary>
        /// Send a buffer through connection
        /// </summary>
        /// <param name="buffer">Buffer</param>
        public void Send(byte[] buffer) {
            _port.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Send an unsigned integer through connection
        /// </summary>
        /// <param name="value">Value</param>
        public void Send(uint value) {
            var bytes = BitConverter.GetBytes(value);

            _port.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Read <paramref name="size"/> bytes from connection
        /// </summary>
        /// <param name="size">Count of bytes to read</param>
        /// <returns>Bytes received</returns>
        public byte[] Read(int size) {
            var bytes = new byte[size];

            // Loop until all bytes are received
            var received = 0;
            do {
                received += _port.Read(bytes, received, size - received);
            } while (received != size);

            return bytes;
        }

        /// <summary>
        /// Open serial connection
        /// </summary>
        public void Open() {
            _port.Open();
        }

        public void Dispose() {
            if (_disposed) {
                return;
            }

            // Dispose connection
            _port.Dispose();
            _disposed = true;

            // Remove from garbage collection
            GC.SuppressFinalize(this);
        }
    }
}