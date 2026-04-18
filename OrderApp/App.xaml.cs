using Microsoft.Extensions.DependencyInjection;
using OrderApp.Services;
using OrderApp.ViewModels;
using OrderApp.Views;
using System.Windows;

namespace OrderApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ServiceProvider _serviceProvider;

    protected override async void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        // Register services.
        services.AddSingleton<IApiService, ApiService>();
        services.AddSingleton<IMessageService, MessageService>();
        services.AddSingleton<MainViewModel>();

        _serviceProvider = services.BuildServiceProvider();

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();

        var mainWindow = new MainWindow
        {
            DataContext = mainViewModel
        };

        mainWindow.Show();

        await mainViewModel.IntializeLoadItemsAsync();

        base.OnStartup(e);
    }
}
