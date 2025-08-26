using CineTrackFE.AppServises;
using CineTrackFE.Common;
using CineTrackFE.Common.Events;
using CineTrackFE.Views.Users;

namespace CineTrackFE.ViewModels.Login;

public class LoginViewModel : BindableBase, INavigationAware
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IAuthService _authService;
    private readonly IRegionManager _regionManager;
    public AsyncDelegateCommand LoginAsyncCommand { get; }
    public AsyncDelegateCommand InstaLoginAsyncCommand { get; }


    public LoginViewModel(IEventAggregator eventAggregator, IAuthService authService, IRegionManager regionManager)
    {
        _eventAggregator = eventAggregator;
        _authService = authService;
        _regionManager = regionManager;
        LoginAsyncCommand = new AsyncDelegateCommand(() => LoginAsync(UserName, Password));
        InstaLoginAsyncCommand = new AsyncDelegateCommand(() => LoginAsync("Test@gmail.com", "123456"));

        _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Login Page");
    }

    private async Task LoginAsync(string userName, string password )
    {
        ErrorMessage = ""; // Reset error message before login attempt

        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
        {
            ErrorMessage = "Username and password cannot be empty.";
            return;
        }

        try
        {
            var IsLoginSuccess = await _authService.LoginAsync(userName, password);
            if (IsLoginSuccess) _regionManager.RequestNavigate(Const.MainRegion, nameof(DashboardView));
            else
            {
                ErrorMessage = "Login failed. Please check your username and password.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred while trying to log in: {ex.Message}";
        }
    }


    // I-NAVIGATION-AWARE //
    public bool IsNavigationTarget(NavigationContext navigationContext) => false;
    public void OnNavigatedTo(NavigationContext navigationContext) { }
    public void OnNavigatedFrom(NavigationContext navigationContext) { }



    // USERNAME //
    private string userName = "";
    public string UserName
    {
        get { return userName; }
        set { SetProperty(ref userName, value); }
    }

    // PASSWORD //
    private string password = "";
    public string Password
    {
        get { return password; }
        set { SetProperty(ref password, value); }
    }


    // ERROR MESSAGE //   
    private string errorMessage = "";
    public string ErrorMessage
    {
        get { return errorMessage; }
        set { SetProperty(ref errorMessage, value); }
    }













}
