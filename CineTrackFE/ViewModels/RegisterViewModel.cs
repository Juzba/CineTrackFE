using CineTrackFE.Common.Events;

namespace CineTrackFE.ViewModels;

public class RegisterViewModel : BindableBase
{
    private readonly IEventAggregator _eventAggregator;

    public RegisterViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Register Page");
    }








}
