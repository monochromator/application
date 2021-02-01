using System.Text.Json.Serialization;

namespace Monochromator.App.Services.Calibration {
    /// <summary>
    /// Calibration arguments
    /// </summary>
    public class CalibrationArguments {
        /// <summary>
        /// Wavelength
        /// </summary>
        [JsonPropertyName("wavelength")]
        public float Wavelength { get; set; }
    }
}