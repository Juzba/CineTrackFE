using CineTrackFE.Common.Events;

namespace CineTrackFE.ViewModels;

public class LoginViewModel : BindableBase
{
    private readonly IEventAggregator _eventAggregator;


    public LoginViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Login Page");
    }



















}
