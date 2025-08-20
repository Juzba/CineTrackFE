using CineTrackFE.AppServises;
using CineTrackFE.Common.Events;

namespace CineTrackFE.ViewModels;

public class LoginViewModel : BindableBase
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IAuthService _authService;
    public AsyncDelegateCommand LoginAsyncCommand { get; }


    public LoginViewModel(IEventAggregator eventAggregator, IAuthService authService )
    {
        _eventAggregator = eventAggregator;
        _authService = authService;
        LoginAsyncCommand = new AsyncDelegateCommand(LoginAsync);

        _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Login Page");
    }

    private async Task LoginAsync()
    {
        try
        {
            var result = await _authService.LoginAsync(UserName, Password);

            // dalsi logika pro prihlaseni 
        }
        catch (Exception)
        {

            throw;
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















}
