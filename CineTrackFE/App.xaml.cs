using CineTrackFE.ViewModels;
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
        base.OnInitialized();
    }


    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();
    }




}
