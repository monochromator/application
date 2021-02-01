using System;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Network;
using Microsoft.Extensions.DependencyInjection;
using Monochromator.App.Exceptions;
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
        /// <param name="configuration">Configuration</param>
        public AnalysisController(IChromelyConfiguration configuration) {
            _service = new AnalysisService(configuration);
        }

        /// <summary>
        /// Callback of /analysis/run
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Response</returns>
        [RequestAction(RouteKey = "/analysis/run")]
        public IChromelyResponse Run(IChromelyRequest request) {
            try {
                // Run analysis
                _service.Analyse(request);

                return new ChromelyResponse();
            } catch (NotCalibratedException e) {
                Logger.Error(e);

                return new ErrorResponse(e, NotCalibratedException.Code);
            } catch (Exception e) {
                Logger.Error(e);

                return new ErrorResponse(e);
            }
        }
    }
}