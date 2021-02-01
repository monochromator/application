using System.Text.Json.Serialization;

namespace Monochromator.App.Services.Analysis {
    /// <summary>
    /// Analysis arguments
    /// </summary>
    public class AnalysisArguments {
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Range start
        /// </summary>
        [JsonPropertyName("start")]
        public float Start { get; set; } = float.NaN;

        /// <summary>
        /// Range end
        /// </summary>
        [JsonPropertyName("end")]
        public float End { get; set; } = float.NaN;

        /// <summary>
        /// Step
        /// </summary>
        [JsonPropertyName("step")]
        public float Step { get; set; } = float.NaN;
    }
}