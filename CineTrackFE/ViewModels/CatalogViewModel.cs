using CineTrackFE.AppServises;
using CineTrackFE.Common.Events;
using CineTrackFE.Models;
using CineTrackFE.Models.DTO;
using System.Collections.ObjectModel;

namespace CineTrackFE.ViewModels;

public class CatalogViewModel : BindableBase, INavigationAware
{
    private readonly IApiService _apiService;
    private readonly IEventAggregator _eventAggregator;


    private readonly AsyncDelegateCommand OnInitializeAsyncCommand;


    public CatalogViewModel(IApiService apiService, IEventAggregator eventAggregator)
    {
        _apiService = apiService;
        _eventAggregator = eventAggregator;


        OnInitializeAsyncCommand = new AsyncDelegateCommand(OnInitializeAsync);
        OnInitializeAsyncCommand.Execute();

        _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Catalog Page");
    }



    // I-NAVIGATION-AWARE //
    public bool IsNavigationTarget(NavigationContext navigationContext) => false;
    public void OnNavigatedTo(NavigationContext navigationContext) { }
    public void OnNavigatedFrom(NavigationContext navigationContext) { }



    // ON INITIALIZE //
    private async Task OnInitializeAsync()
    {
        ErrorMessage = null;

        try
        {
            var filmListDb = await _apiService.PostAsync<IEnumerable<Film>, SearchParametrsDto>("/api/FilmApi/CatalogSearch", null!);
            if (filmListDb != null) FilmList = new ObservableCollection<Film>(filmListDb);


            var genreListDb = await _apiService.GetAsync<IEnumerable<Genre>>("/api/FilmApi/AllGenres");
            if (genreListDb != null) GenresList = new ObservableCollection<Genre>(genreListDb);

        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            // chyba
        }

    }

    // FILM LIST //
    private ObservableCollection<Film> filmList = [];
    public ObservableCollection<Film> FilmList
    {
        get { return filmList; }
        set { SetProperty(ref filmList, value); }
    }


    // GENRES //
    private ObservableCollection<Genre> genresList = [];
    public ObservableCollection<Genre> GenresList
    {
        get { return genresList; }
        set { SetProperty(ref genresList, value); }
    }


    // SEARCH TEXT //
    private string? searchText;
    public string? SearchText
    {
        get { return searchText; }
        set { SetProperty(ref searchText, value); }
    }


    // SELECTED SEARCH ORDER-BY//
    private string? searchOrder;
    public string? SearchOrder
    {
        get { return searchOrder; }
        set { SetProperty(ref searchOrder, value); }
    }


    // SELECTED GENRE //
    private Genre? searchByGenre;
    public Genre? SearchByGenre
    {
        get { return searchByGenre; }
        set { SetProperty(ref searchByGenre, value); }
    }


    // SEARCH BY YEAR //
    private int? searchByYear;
    public int? SearchByYear
    {
        get { return searchByYear; }
        set { SetProperty(ref searchByYear, value); }
    }











    private string? errorMessage;
    public string? ErrorMessage
    {
        get { return errorMessage; }
        set { SetProperty(ref errorMessage, value); }
    }



}
