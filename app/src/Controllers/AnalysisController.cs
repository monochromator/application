using System;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Network;
using Monochromator.App.Services.Analysis;
using NLog;

namespace Monochromator.App.Controllers {
    /// <summary>
    /// Controller for analysis
    /// </summary>
    public class AnalysisController : ChromelyController {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly AnalysisService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Container</param>
        /// <param name="configuration">Configuration</param>
        public AnalysisController(IChromelyContainer container, IChromelyConfiguration configuration) {
            _service = new AnalysisService(container, configuration);
        }

        // TODO: Calibration

        /// <summary>
        /// Callback of /analysis/run
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Response</returns>
        [HttpPost(Route = "/analysis/run")]
        public ChromelyResponse Run(ChromelyRequest request) {
            try {
                // Run analysis
                _service.Analyse(request);

                return new ChromelyResponse();
            } catch (Exception e) {
                Logger.Error(e);

                return new ErrorResponse(e);
            }
        }
    }
}