using CineTrackFE.AppServises;
using CineTrackFE.Common.Events;
using CineTrackFE.Models;
using System.Collections.ObjectModel;

namespace CineTrackFE.ViewModels;

public class FilmViewModel : BindableBase, INavigationAware
{
    private readonly IApiService _apiService;
    private readonly IEventAggregator _eventAggregator;
    private readonly AsyncDelegateCommand _OnInitializeAsyncCommand;

    public FilmViewModel(IApiService apiService, IEventAggregator eventAggregator)
    {
        _apiService = apiService;
        _eventAggregator = eventAggregator;

        _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Movies Page");


        _OnInitializeAsyncCommand = new AsyncDelegateCommand(OnInitializeAsync);
        _OnInitializeAsyncCommand.Execute();
    }


    // I-NAVIGATION-AWARE //
    public bool IsNavigationTarget(NavigationContext navigationContext) => false;
    public void OnNavigatedFrom(NavigationContext navigationContext) { }
    public void OnNavigatedTo(NavigationContext navigationContext) { }



    private async Task OnInitializeAsync()
    {
        var filmsFromDb = await _apiService.GetAsync<IEnumerable<Film>>("/api/FilmApi/Test");
        if (filmsFromDb != null) 
        { 
            FilmList = new ObservableCollection<Film>(filmsFromDb); 
        }

    }



    private ObservableCollection<Film> filmList = [];
    public ObservableCollection<Film> FilmList
    {
        get { return filmList; }
        set { SetProperty(ref filmList, value); }
    }









}
