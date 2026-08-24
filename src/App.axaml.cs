using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using QuickQR.Configs;
using QuickQR.Services;
using QuickQR.ViewModels;
using QuickQR.Views;
using SukiUI.Dialogs;
using SukiUI.Toasts;

namespace QuickQR;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var services = new ServiceCollection();
            services.AddSingleton(desktop);
            var views = ConfigureViews(services);
            var provider = ConfigureServices(services);

            var mainWindow = views.CreateView<WindowViewModel>(provider) as Window;
            desktop.MainWindow = mainWindow;

            // MVP has a single page, so wire it up directly as the window's content.
            if (mainWindow?.DataContext is WindowViewModel windowViewModel)
            {
                windowViewModel.CurrentViewModel = provider.GetRequiredService<QrGeneratorViewModel>();
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static SukiViews ConfigureViews(ServiceCollection services)
    {
        return new SukiViews()

            // Add main view
            .AddView<WindowView, WindowViewModel>(services)
            .AddView<QrGeneratorView, QrGeneratorViewModel>(services);
    }

    private static ServiceProvider ConfigureServices(ServiceCollection services)
    {        
        services.AddSingleton<ISukiToastManager, SukiToastManager>();
        services.AddSingleton<ISukiDialogManager, SukiDialogManager>();
        services.AddSingleton<IQrCodeService, QrCodeService>();

        return services.BuildServiceProvider();
    }
}