using CommunityToolkit.Mvvm.ComponentModel;
using QuickQR.Configs;
using SukiUI.Toasts;

namespace QuickQR.ViewModels;

public partial class WindowViewModel(ISukiToastManager toastManager) : ViewModelBase
{
    public ISukiToastManager ToastManager { get; } = toastManager;

    [ObservableProperty] private ViewModelBase? _currentViewModel;
}
