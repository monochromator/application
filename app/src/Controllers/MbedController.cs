using System;
using Chromely.Core;
using Chromely.Core.Network;
using Monochromator.App.Mbed;
using Monochromator.App.Services.Mbed;
using NLog;

namespace Monochromator.App.Controllers {
    /// <summary>
    /// Controller for MBED
    /// </summary>
    public class MbedController : ChromelyController {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly MbedService _mbedService;
        private readonly IChromelyContainer _container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public MbedController(IChromelyContainer container) {
            _container = container;
            _mbedService = new MbedService(_container);
        }

        /// <summary>
        /// List connected controllers
        /// </summary>
        /// <param name="_">Request</param>
        /// <returns>Response</returns>
        [HttpGet(Route = "/mbed/list")]
        public ChromelyResponse ListControllers(ChromelyRequest _) {
            try {
                return new ChromelyResponse {
                    Data = _mbedService.Controllers()
                };
            } catch (Exception e) {
                Logger.Error(e);

                return new ErrorResponse(e);
            }
        }

        /// <summary>
        /// Autodetect monochromator controller
        /// </summary>
        /// <param name="_">Request</param>
        /// <returns>Response</returns>
        [HttpGet(Route = "/mbed/autodetect")]
        public ChromelyResponse Autodetect(ChromelyRequest _) {
            try {
                return new ChromelyResponse {
                    Data = _mbedService.Autodetect()
                };
            } catch (Exception e) {
                Logger.Error(e);

                return new ErrorResponse(e);
            }
        }

        /// <summary>
        /// Connect to the given device
        /// </summary>
        /// <param name="request">Connection request</param>
        /// <returns>Response</returns>
        [HttpGet(Route = "/mbed/connect")]
        public ChromelyResponse Connect(ChromelyRequest request) {
            try {
                // Discard old connection
                _container.GetInstance<SerialConnection>(typeof(SerialConnection).FullName)?.Dispose();

                // Connect
                var conn = _mbedService.Connect(request);

                // Save connection
                _container.RegisterInstance(typeof(SerialConnection).FullName, conn);

                return new ChromelyResponse();
            } catch (Exception e) {
                Logger.Error(e);

                return new ErrorResponse(e);
            }
        }

        /// <summary>
        /// Ping connected controller
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Response</returns>
        [HttpGet(Route = "/mbed/current/ping")]
        public ChromelyResponse Ping(ChromelyRequest request) {
            try {
                _mbedService.Ping();
                return new ChromelyResponse();
            } catch (Exception e) {
                Logger.Error(e);
                
                return new ErrorResponse(e);
            }
        }

        /// <summary>
        /// Disconnect connected controller
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Response</returns>
        [HttpGet(Route = "/mbed/current/disconnect")]
        public ChromelyResponse Disconnect(ChromelyRequest request) {
            try {
                // Drop old connection
                _container.RegisterInstance<SerialConnection?>(typeof(SerialConnection).FullName, null);

                return new ChromelyResponse();
            } catch (Exception e) {
                Logger.Error(e);

                return new ErrorResponse(e);
            }
        }
    }
}