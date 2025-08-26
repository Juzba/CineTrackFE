using CineTrackFE.AppServises;
using CineTrackFE.Common;
using CineTrackFE.ViewModels;
using CineTrackFE.ViewModels.Admin;
using CineTrackFE.ViewModels.Login;
using CineTrackFE.ViewModels.Users;
using CineTrackFE.Views.Login;
using CineTrackFE.Views.Admin;
using CineTrackFE.Views.Users;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Net.Http;
using System.Windows;
using CineTrackFE.Views;

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
        containerRegistry.RegisterSingleton<UserStore>();



        containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
        containerRegistry.RegisterForNavigation<RegisterView, RegisterViewModel>();
        containerRegistry.RegisterForNavigation<CatalogView, CatalogViewModel>();
        containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>();
        containerRegistry.RegisterForNavigation<FilmDetailView, FilmDetailViewModel>();
        containerRegistry.RegisterForNavigation<UserProfilView, UserProfilViewModel>();
        containerRegistry.RegisterForNavigation<MainAdminView, MainAdminViewModel>();
        // Admin Views
        containerRegistry.RegisterForNavigation<FilmEditView, FilmEditViewModel>();
        containerRegistry.RegisterForNavigation<GenreEditView, GenreEditViewModel>();
        containerRegistry.RegisterForNavigation<UsersEditView, UsersEditViewModel>();
        containerRegistry.RegisterForNavigation<WebStatisticView, WebStatisticViewModel>();


        // Register HttpClient for API calls
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7238/")
        };
        containerRegistry.RegisterInstance(httpClient);
        containerRegistry.Register<IApiService, ApiService>();
        containerRegistry.RegisterSingleton<IAuthService, AuthService>();
    }



    protected override Window CreateShell()
    {
        var mainWindow = Container.Resolve<MainWindow>();
        mainWindow.DataContext = Container.Resolve<MainViewModel>();
        mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        mainWindow.Show();

        return mainWindow;
    }

    protected override void OnInitialized()
    {
        var regionManager = Container.Resolve<IRegionManager>();
        //first page to show
        regionManager.RequestNavigate(Const.MainRegion, nameof(LoginView));


        base.OnInitialized();
    }



    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();
    }




}
