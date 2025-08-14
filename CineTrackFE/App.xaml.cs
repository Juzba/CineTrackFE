using CineTrackFE.AppServises;
using CineTrackFE.Common;
using CineTrackFE.ViewModels;
using CineTrackFE.Views;
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
        containerRegistry.Register<IApiService, ApiService>();


        containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
        containerRegistry.RegisterForNavigation<RegisterView, RegisterViewModel>();
        containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>();
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

        regionManager.RequestNavigate(Const.MainRegion, nameof(LoginView));


        base.OnInitialized();
    }


    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();
    }




}
