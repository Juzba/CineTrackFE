using CineTrackFE.AppServises;
using CineTrackFE.Common;
using CineTrackFE.Common.Events;
using CineTrackFE.Models;
using CineTrackFE.Models.DTO;
using CineTrackFE.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace CineTrackFE.ViewModels;

public class CatalogViewModel : BindableBase, INavigationAware
{
    private readonly IApiService _apiService;
    private readonly IEventAggregator _eventAggregator;
    private readonly IRegionManager _regionManager;


    private readonly AsyncDelegateCommand OnInitializeAsyncCommand;
    private readonly AsyncDelegateCommand SendSearchDataAsyncCommand;

    public DelegateCommand SearchFilterResetCommand { get; }
    public DelegateCommand<Film> OpenFilmDetailsCommand { get; }


    public CatalogViewModel(IApiService apiService, IEventAggregator eventAggregator, IRegionManager regionManager)
    {
        _apiService = apiService;
        _eventAggregator = eventAggregator;
        _regionManager = regionManager;


        OnInitializeAsyncCommand = new AsyncDelegateCommand(OnInitializeAsync);
        SendSearchDataAsyncCommand = new AsyncDelegateCommand(SendSearchDataAsync);
        OpenFilmDetailsCommand = new DelegateCommand<Film>(film =>
        {
            if (film != null)
            {
                _regionManager.RequestNavigate(Const.MainRegion, nameof(FilmDetailView), new NavigationParameters() { {Const.FilmId, film.Id } });
            }
        });
        SearchFilterResetCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(CatalogView)));


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
        await LoadFilmsFromApi();
        await LoadGenresFromApi();
    }

    // LOAD FILMS FROM API - DB //
    private async Task LoadFilmsFromApi()
    {
        try
        {
            var filmListDb = await _apiService.PostAsync<IEnumerable<Film>, SearchParametrsDto>("/api/FilmApi/CatalogSearch", searchParametrs);
            if (filmListDb != null) FilmList = new ObservableCollection<Film>(filmListDb);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        FilmCountMessage = FilmList.Count.ToString();
    }


    // LOAD GENRES FROM API - DB //
    private async Task LoadGenresFromApi()
    {
        try
        {
            var genreListDb = await _apiService.GetAsync<IEnumerable<Genre>>("/api/FilmApi/AllGenres");
            if (genreListDb != null) GenresList.AddRange(new ObservableCollection<Genre>(genreListDb));

        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }


    private async Task SendSearchDataAsync()
    {
        ErrorMessage = null;
        await LoadFilmsFromApi();
        ButtonResetVisibility = Visibility.Visible;

    }


    // FILM LIST //
    private ObservableCollection<Film> filmList = [];
    public ObservableCollection<Film> FilmList
    {
        get { return filmList; }
        set { SetProperty(ref filmList, value); }
    }


    // GENRES LIST //
    private ObservableCollection<Genre> genresList = [new Genre() { Id = 0, Name = "Všechny" }];
    public ObservableCollection<Genre> GenresList
    {
        get { return genresList; }
        set { SetProperty(ref genresList, value); }
    }


    // SEARCH PARAMETRS DTO //
    private SearchParametrsDto searchParametrs = new();
    public SearchParametrsDto SearchParametrs
    {
        get { return searchParametrs; }
        set { searchParametrs = value; }
    }


    // SEARCH TEXT //
    private string? searchText;
    public string? SearchText
    {
        get { return searchText; }
        set
        {
            SetProperty(ref searchText, value);
            searchParametrs.SearchText = searchText;
            SendSearchDataAsyncCommand.Execute();
        }
    }


    // SELECTED SEARCH ORDER-BY//
    private string? searchOrder;
    public string? SearchOrder
    {
        get { return searchOrder; }
        set
        {
            SetProperty(ref searchOrder, value);
            searchParametrs.SearchOrder = value;
            SendSearchDataAsyncCommand.Execute();
        }
    }


    // SELECTED GENRE //
    private Genre? searchByGenre;
    public Genre? SearchByGenre
    {
        get { return searchByGenre; }
        set
        {
            SetProperty(ref searchByGenre, value);
            searchParametrs.GenreId = value?.Id;
            SendSearchDataAsyncCommand.Execute();
        }
    }

    // SEARCH BY YEAR //
    private string? searchByYear;
    public string? SearchByYear
    {
        get { return searchByYear; }
        set
        {
            SetProperty(ref searchByYear, value);
            if (int.TryParse(value, out int intYear))
            {
                searchParametrs.SearchByYear = intYear;
                SendSearchDataAsyncCommand.Execute();
            }
            else if (string.IsNullOrWhiteSpace(value))
            {
                searchParametrs.SearchByYear = null;
                SendSearchDataAsyncCommand.Execute();
            }
        }
    }

    // ERROR MESSAGE //
    private string? errorMessage;
    public string? ErrorMessage
    {
        get { return errorMessage; }
        set { SetProperty(ref errorMessage, value); }
    }


    // FILM COUNT MESSAGE //
    private string? filmCountMessage;
    public string? FilmCountMessage
    {
        get { return filmCountMessage; }
        set
        {
            var result = $"Nalezeno {value} filmů.";
            SetProperty(ref filmCountMessage, result);
        }
    }


    // BUTTON FILTER-RESET VISIBILITY //
    private Visibility buttonResetVisibility = Visibility.Hidden;
    public Visibility ButtonResetVisibility
    {
        get { return buttonResetVisibility; }
        set { SetProperty(ref buttonResetVisibility, value); }
    }




}
