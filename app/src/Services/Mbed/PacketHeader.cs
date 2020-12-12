namespace Monochromator.App.Services.Mbed {
    /// <summary>
    /// Header sent before common packet body
    /// </summary>
    public enum PacketHeader : uint {
        Ping = 0,
        Analysis = 1
    }
}