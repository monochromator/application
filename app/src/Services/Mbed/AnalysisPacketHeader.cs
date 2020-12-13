namespace Monochromator.App.Services.Mbed {
    /// <summary>
    /// Header sent before analysis-specific packet body
    /// </summary>
    public enum AnalysisPacketHeader : uint {
        InvalidArguments = 0,
        Start = 1,
        ResultsSize = 2
    }
}