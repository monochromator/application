namespace Monochromator.App.Services.Mbed {
    /// <summary>
    /// Header sent before calibrate-specific packet body
    /// </summary>
    public enum CalibratePacketHeader {
        InvalidArguments = 0,
        Start = 1,
        End = 2
    }
}