using System.Text.Json.Serialization;

namespace Monochromator.App.Services.Analysis {
    /// <summary>
    /// Analysis arguments
    /// </summary>
    public class AnalysisArguments {
        /// <summary>
        /// Range start
        /// </summary>
        [JsonPropertyName("start")]
        public float Start { get; set; }

        /// <summary>
        /// Range end
        /// </summary>
        [JsonPropertyName("end")]
        public float End { get; set; }

        /// <summary>
        /// Step
        /// </summary>
        [JsonPropertyName("step")]
        public float Step { get; set; }
    }
}