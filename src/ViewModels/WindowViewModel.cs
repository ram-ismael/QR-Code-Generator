using CommunityToolkit.Mvvm.ComponentModel;
using QuickQR.Configs;
using SukiUI.Dialogs;
using SukiUI.Toasts;

namespace QuickQR.ViewModels;

public partial class WindowViewModel(
    ISukiToastManager toastManager,
    ISukiDialogManager dialogManager) : ViewModelBase
{
    public ISukiToastManager ToastManager { get; } = toastManager;
    public ISukiDialogManager DialogManager { get; } = dialogManager;

    [ObservableProperty] private ViewModelBase? _currentViewModel;
}
