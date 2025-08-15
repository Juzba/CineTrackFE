using CineTrackFE.AppServises;
using CineTrackFE.Common;
using CineTrackFE.ViewModels;
using CineTrackFE.Views;
using Microsoft.Xaml.Behaviors.Core;
using System.Net.Http;
using System.Windows;

namespace CineTrackFE;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<MainWindow>();
        containerRegistry.Register<MainViewModel>();



        containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
        containerRegistry.RegisterForNavigation<RegisterView, RegisterViewModel>();
        containerRegistry.RegisterForNavigation<FilmView, FilmViewModel>();
        containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>();


        // Register HttpClient for API calls
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7238/")
        };
        containerRegistry.RegisterInstance(httpClient);
        containerRegistry.Register<IApiService, ApiService>();
    }



    protected override Window CreateShell()
    {
        var mainWindow = Container.Resolve<MainWindow>();
        mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        mainWindow.Show();
        mainWindow.DataContext = Container.Resolve<MainViewModel>();

        return mainWindow;
    }

    protected override void OnInitialized()
    {
        var regionManager = Container.Resolve<IRegionManager>();

        regionManager.RequestNavigate(Const.MainRegion, nameof(DashboardView));


        base.OnInitialized();
    }


    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();
    }




}
