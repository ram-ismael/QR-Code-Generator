using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickQR.Configs;
using QuickQR.Services;
using SukiUI.Toasts;

namespace QuickQR.ViewModels;

public partial class QrGeneratorViewModel(IQrCodeService qrCodeService, ISukiToastManager toastManager) : ViewModelBase
{
    private readonly IQrCodeService _qrCodeService = qrCodeService;
    private readonly ISukiToastManager _toastManager = toastManager;

    private CancellationTokenSource? _debounceCts;
    private byte[]? _lastPngBytes;

    /// <summary>
    /// Set by the view once attached to a TopLevel, so the "Save as PNG" command
    /// can open a native save-file dialog.
    /// </summary>
    public IStorageProvider? StorageProvider { get; set; }

    public Array EccLevels { get; } = Enum.GetValues(typeof(QrErrorCorrectionLevel));

    [ObservableProperty]
    private string _inputText = string.Empty;

    [ObservableProperty]
    private Bitmap? _qrImage;

    [ObservableProperty]
    private string _statusMessage = "Type or paste text/a URL above to generate a QR code.";

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private QrErrorCorrectionLevel _selectedEccLevel = QrErrorCorrectionLevel.Quartile;

    [ObservableProperty]
    private double _moduleSize = 12;

    partial void OnInputTextChanged(string value) => QueueGenerate();

    partial void OnSelectedEccLevelChanged(QrErrorCorrectionLevel value) => QueueGenerate();

    partial void OnModuleSizeChanged(double value) => QueueGenerate();

    /// <summary>
    /// Debounces regeneration slightly so rapid typing / slider dragging doesn't
    /// re-encode the QR code on every single change.
    /// </summary>
    private void QueueGenerate()
    {
        _debounceCts?.Cancel();
        var cts = new CancellationTokenSource();
        _debounceCts = cts;

        _ = DebouncedGenerateAsync(cts.Token);
    }

    private async Task DebouncedGenerateAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(150, token);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        if (token.IsCancellationRequested)
        {
            return;
        }

        Dispatcher.UIThread.Post(Generate);
    }

    [RelayCommand]
    private void Generate()
    {
        SaveAsPngCommand.NotifyCanExecuteChanged();

        if (string.IsNullOrWhiteSpace(InputText))
        {
            QrImage = null;
            _lastPngBytes = null;
            HasError = false;
            StatusMessage = "Type or paste text/a URL above to generate a QR code.";
            SaveAsPngCommand.NotifyCanExecuteChanged();
            return;
        }

        try
        {
            var pixelsPerModule = Math.Max(1, (int)Math.Round(ModuleSize));
            var pngBytes = _qrCodeService.GeneratePng(InputText, SelectedEccLevel, pixelsPerModule);

            using var stream = new MemoryStream(pngBytes);
            var bitmap = new Bitmap(stream);

            QrImage?.Dispose();
            QrImage = bitmap;
            _lastPngBytes = pngBytes;

            HasError = false;
            StatusMessage = $"QR code ready — {InputText.Length} character(s), {bitmap.PixelSize.Width}×{bitmap.PixelSize.Height}px.";
        }
        catch (Exception ex)
        {
            QrImage = null;
            _lastPngBytes = null;
            HasError = true;
            StatusMessage = $"Couldn't generate QR code: {ex.Message}";
        }
        finally
        {
            SaveAsPngCommand.NotifyCanExecuteChanged();
        }
    }

    private const int ExportPixelsPerModule = 40; // ~ resolução alta para exportação

    private bool CanSaveAsPng() => !string.IsNullOrWhiteSpace(InputText) && !HasError;

    [RelayCommand(CanExecute = nameof(CanSaveAsPng))]
    private async Task SaveAsPngAsync()
    {
        if (string.IsNullOrWhiteSpace(InputText))
        {
            return;
        }

        if (StorageProvider is null)
        {
            StatusMessage = "Save isn't available on this window yet.";
            return;
        }

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save QR Code",
            SuggestedFileName = "quick_qr_code.png",
            DefaultExtension = "png",
            FileTypeChoices =
            [
                new FilePickerFileType("PNG Image") { Patterns = ["*.png"] }
            ]
        });

        if (file is null)
        {
            return;
        }

        try
        {
            // Gera uma versão de alta resolução separada do preview, em vez de reaproveitar _lastPngBytes
            var highResPngBytes = _qrCodeService.GeneratePng(InputText, SelectedEccLevel, ExportPixelsPerModule);

            await using var stream = await file.OpenWriteAsync();
            await stream.WriteAsync(highResPngBytes);

            _toastManager.CreateToast()
                .WithTitle("Saved")
                .WithContent($"QR code saved to {file.Name}")
                .OfType(NotificationType.Success)
                .Dismiss().After(TimeSpan.FromSeconds(5))
                .Dismiss().ByClicking()
                .Queue();
        }
        catch (Exception ex)
        {
            _toastManager.CreateToast()
                .WithTitle("Save failed")
                .WithContent(ex.Message)
                .OfType(NotificationType.Error)
                .Dismiss().After(TimeSpan.FromSeconds(5))
                .Dismiss().ByClicking()
                .Queue();
        }
    }
}
