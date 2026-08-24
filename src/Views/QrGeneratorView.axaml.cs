using Avalonia.Controls;
using QuickQR.ViewModels;

namespace QuickQR.Views;

public partial class QrGeneratorView : UserControl
{
    public QrGeneratorView()
    {
        InitializeComponent();
        AttachedToVisualTree += OnAttachedToVisualTree;
    }

    private void OnAttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
    {
        if (DataContext is QrGeneratorViewModel vm)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            vm.StorageProvider = topLevel?.StorageProvider;
        }
    }
}
