namespace QuickQR.Services;

/// <summary>
/// Error correction level for generated QR codes. Higher levels tolerate more
/// damage/obstruction but produce denser codes.
/// </summary>
public enum QrErrorCorrectionLevel
{
    Low,
    Medium,
    Quartile,
    High
}

public interface IQrCodeService
{
    /// <summary>
    /// Encodes <paramref name="content"/> as a QR code and renders it to PNG bytes.
    /// </summary>
    /// <param name="content">Text/URL to encode. Cannot be null or whitespace.</param>
    /// <param name="eccLevel">Error correction level.</param>
    /// <param name="pixelsPerModule">Size in pixels of each QR module (must be >= 1).</param>
    byte[] GeneratePng(string content, QrErrorCorrectionLevel eccLevel, int pixelsPerModule);
}
