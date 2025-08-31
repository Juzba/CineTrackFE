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
        LoginAsyncCommand = new AsyncDelegateCommand(() => LoginAsync(Email, Password));
        InstaLoginAsyncCommand = new AsyncDelegateCommand(() => LoginAsync("Test@gmail.com", "123456"));

        _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Login Page");
    }

    private async Task LoginAsync(string Email, string password)
    {
        ErrorMessage = "";

        if (string.IsNullOrWhiteSpace(Email))
        {
            ErrorMessage = "Email cannot be empty.";
            return;
        }

        if (!Email.Contains('@') || !Email.Contains('.'))
        {
            ErrorMessage = "Please enter a valid email address.";
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            ErrorMessage = "Password cannot be empty.";
            return;
        }

        if (password.Length < 6)
        {
            ErrorMessage = "Password must be at least 6 characters long.";
            return;
        }

        try
        {
            var IsLoginSuccess = await _authService.LoginAsync(Email, password);
            if (IsLoginSuccess) _regionManager.RequestNavigate(Const.MainRegion, nameof(DashboardView));
            else
            {
                ErrorMessage = "Login failed. Please check your Email and Password.";
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



    // EMAIL //
    private string email = string.Empty;
    public string Email
    {
        get { return email; }
        set { SetProperty(ref email, value); }
    }

    // PASSWORD //
    private string password = string.Empty;
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
