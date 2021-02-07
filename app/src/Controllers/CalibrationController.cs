using System;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Network;
using Monochromator.App.Services.Calibration;
using NLog;

namespace Monochromator.App.Controllers {
    /// <summary>
    /// Controller for calibration
    /// </summary>
    public class CalibrationController : ChromelyController {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly CalibrationService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public CalibrationController(IChromelyConfiguration configuration) {
            _service = new CalibrationService(configuration);
        }

        /// <summary>
        /// Callback of /calibration/run
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Response</returns>
        [HttpPost(Route = "/calibration/run")]
        public ChromelyResponse Run(ChromelyRequest request) {
            try {
                // Run analysis
                _service.Calibrate(request);

                return new ChromelyResponse();
            } catch (Exception e) {
                Logger.Error(e);

                return new ErrorResponse(e);
            }
        }
    }
}