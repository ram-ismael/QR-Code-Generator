using System;
using QRCoder;

namespace QuickQR.Services;

/// <summary>
/// Generates QR code PNG images using QRCoder's <see cref="PngByteQRCode"/> renderer,
/// which is pure managed code (no System.Drawing / libgdiplus dependency), so it works
/// on Windows, Linux and macOS without extra native packages.
/// </summary>
public sealed class QrCodeService : IQrCodeService
{
    public byte[] GeneratePng(string content, QrErrorCorrectionLevel eccLevel, int pixelsPerModule)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be empty.", nameof(content));
        }

        if (pixelsPerModule < 1)
        {
            pixelsPerModule = 1;
        }

        var generator = new QRCodeGenerator();
        var qrCodeData = generator.CreateQrCode(content, MapLevel(eccLevel));
        var pngRenderer = new PngByteQRCode(qrCodeData);

        return pngRenderer.GetGraphic(pixelsPerModule);
    }

    private static QRCodeGenerator.ECCLevel MapLevel(QrErrorCorrectionLevel level) => level switch
    {
        QrErrorCorrectionLevel.Low => QRCodeGenerator.ECCLevel.L,
        QrErrorCorrectionLevel.Medium => QRCodeGenerator.ECCLevel.M,
        QrErrorCorrectionLevel.Quartile => QRCodeGenerator.ECCLevel.Q,
        QrErrorCorrectionLevel.High => QRCodeGenerator.ECCLevel.H,
        _ => QRCodeGenerator.ECCLevel.Q
    };
}
