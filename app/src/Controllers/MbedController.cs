using System;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Network;
using Monochromator.App.Mbed;
using Monochromator.App.Services.Mbed;
using NLog;

namespace Monochromator.App.Controllers {
    // TODO: Doc
    public class MbedController : ChromelyController {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly MbedService _mbedService = new MbedService();
        private readonly IChromelyConfiguration _config;
        private readonly IChromelyContainer _container;

        // TODO: Doc
        public MbedController(IChromelyConfiguration config, IChromelyContainer container) {
            _config = config;
            _container = container;
        }

        // TODO: Doc
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

        // TODO: Doc
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

        // TODO: Doc
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
    }
}