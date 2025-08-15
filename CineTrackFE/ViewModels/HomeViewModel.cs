using CineTrackFE.AppServises;
using CineTrackFE.Common.Events;
using System.Windows;

namespace CineTrackFE.ViewModels;

public class HomeViewModel : BindableBase
{
    private readonly IApiService _apiService;
    private readonly IEventAggregator _eventAggregator;
    public AsyncDelegateCommand ClickCommand { get; }



    public HomeViewModel(IApiService apiService, IEventAggregator eventAggregator)
    {
        _apiService = apiService;
        _eventAggregator = eventAggregator;

        _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Home Page");



        ClickCommand = new AsyncDelegateCommand(Click);
    }




    private async Task Click()
    {
        var neco = await _apiService.GetAsync<IEnumerable<string>>("/api/FilmApi/Test");
        MessageBox.Show(neco?.FirstOrDefault()?? "Nic nenalezeno");
    }









}
