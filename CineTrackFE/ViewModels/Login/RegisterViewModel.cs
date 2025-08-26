using CineTrackFE.AppServises;
using CineTrackFE.Common.Events;

namespace CineTrackFE.ViewModels.Login;

public class RegisterViewModel : BindableBase, INavigationAware
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IAuthService _authService;
    private readonly IRegionManager _regionManager;

    public AsyncDelegateCommand RegisterCommand { get; }

    public RegisterViewModel(IEventAggregator eventAggregator, IAuthService authService, IRegionManager regionManager)
    {
        _eventAggregator = eventAggregator;
        _authService = authService;
        _regionManager = regionManager;

        RegisterCommand = new AsyncDelegateCommand(Register, () => CanRegister);

        _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Register Page");
    }


    // I-NAVIGATION-AWARE //
    public bool IsNavigationTarget(NavigationContext navigationContext) => false;
    public void OnNavigatedTo(NavigationContext navigationContext) { }
    public void OnNavigatedFrom(NavigationContext navigationContext) { }



    private async Task Register()
    {
        ErrorMessage = ""; // Clear previous error message

        if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ErrorMessage = "All fields are required.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return;
        }

        try
        {
            CanRegister = false; // Disable the button to prevent multiple clicks
            var result = await _authService.RegisterAsync(UserName, Password);

            if (result)
                _regionManager.RequestNavigate("MainRegion", "LoginView");
            else
                ErrorMessage = "Registration failed. Please try again.";

        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
        }

        finally
        {
            CanRegister = true; // Re-enable the button
        }
    }


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



    // CONFIRM PASSWORD //  
    private string confirmPassword = "";
    public string ConfirmPassword
    {
        get { return confirmPassword; }
        set { SetProperty(ref confirmPassword, value); }
    }




    // ERROR MESSAGE //
    private string errorMessage = "";
    public string ErrorMessage
    {
        get { return errorMessage; }
        set { SetProperty(ref errorMessage, value); }
    }



    // CAN REGISTER //
    private bool canRegister = true;
    public bool CanRegister
    {
        get { return canRegister; }
        set { SetProperty(ref canRegister, value); }
    }


}
