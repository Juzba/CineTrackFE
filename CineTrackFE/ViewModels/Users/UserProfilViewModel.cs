using CineTrackFE.AppServises;
using CineTrackFE.Common.Events;
using CineTrackFE.Models;
using System.Collections.ObjectModel;

namespace CineTrackFE.ViewModels.Users;

public class UserProfilViewModel : BindableBase, IRegionAware
{



    private readonly IApiService _apiService;
    private readonly IEventAggregator _eventAggregator;


    private readonly AsyncDelegateCommand OnInitializeAsyncCommand;


    public UserProfilViewModel(IApiService apiService, IEventAggregator eventAggregator)
    {
        _apiService = apiService;
        _eventAggregator = eventAggregator;


        OnInitializeAsyncCommand = new AsyncDelegateCommand(OnInitializeAsync);
        OnInitializeAsyncCommand.Execute();

        _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("User Profil");
    }



    // I-NAVIGATION-AWARE //
    public bool IsNavigationTarget(NavigationContext navigationContext) => false;
    public void OnNavigatedTo(NavigationContext navigationContext) { }
    public void OnNavigatedFrom(NavigationContext navigationContext) { }



    // ON INITIALIZE //
    private async Task OnInitializeAsync()
    {
        await GetUserData();
    }


    // GET USER DATA //
    private async Task GetUserData()
    {
        try
        {
            var userData = await _apiService.GetAsync<UserProfilData>("/Api/FilmApi/UserProfilData");
            if (userData != null)
            {
                UserFavoriteFilms = new ObservableCollection<FavoriteFilms>(userData.FavoriteFilms);
                UserLatestComments = new ObservableCollection<LatestComment>(userData.LatestComments);
                AvgRating = userData.AvgRating;
                TopRating = userData.TopRating;
                TotalComments = userData.TotalComments;
                FavoriteFilmsCount = userData.FavoriteFilmsCount;
                LastFavoriteFilmTitle = userData.LastFavoriteFilmTitle;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;

        }
    }


    // USER FAVORITE FILMS //
    private ObservableCollection<FavoriteFilms> userFavoriteFilms = [];
    public ObservableCollection<FavoriteFilms> UserFavoriteFilms
    {
        get { return userFavoriteFilms; }
        set { SetProperty(ref userFavoriteFilms, value); }
    }


    // USER LAST COMMENTS //
    private ObservableCollection<LatestComment> userLatestComments = [];
    public ObservableCollection<LatestComment> UserLatestComments
    {
        get { return userLatestComments; }
        set { SetProperty(ref userLatestComments, value); }
    }


    // ERROR MESSAGE //
    private string errorMessage = string.Empty;
    public string ErrorMessage
    {
        get { return errorMessage; }
        set { SetProperty(ref errorMessage, value); }
    }


    // AVG RATING //
    private int avgRating;
    public int AvgRating
    {
        get { return avgRating; }
        set { SetProperty(ref avgRating, value); }
    }


    // TOP RATING //
    private int topRating;
    public int TopRating
    {
        get { return topRating; }
        set { SetProperty(ref topRating, value); }
    }

    // TOTAL COMMENTS //
    private int totalComments;
    public int TotalComments
    {
        get { return totalComments; }
        set { SetProperty(ref totalComments, value); }
    }

    // FAVORITE FILMS COUNT //
    private int favoriteFilmsCount;
    public int FavoriteFilmsCount
    {
        get { return favoriteFilmsCount; }
        set { SetProperty(ref favoriteFilmsCount, value); }
    }

    // LAST FAVORITE FILM //
    private string lastFavoriteFilmTitle = string.Empty;
    public string LastFavoriteFilmTitle
    {
        get { return lastFavoriteFilmTitle; }
        set { SetProperty(ref lastFavoriteFilmTitle, value); }
    }



}
