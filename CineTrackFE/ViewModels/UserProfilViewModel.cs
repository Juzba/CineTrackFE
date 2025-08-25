using CineTrackFE.AppServises;
using CineTrackFE.Common.Events;
using System.Collections.ObjectModel;

namespace CineTrackFE.ViewModels;

public class UserProfilViewModel : BindableBase, IRegionAware
{

    // 4. Uživatelský profil
    //- Přehled aktivit uživatele(nedávná hodnocení, komentáře)
    //- Seznam oblíbených filmů
    //- Statistiky uživatele(počet zhlédnutých filmů, průměrné hodnocení)
    //- Možnost upravit osobní údaje a preference


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

    }


    // USER FAVORITE FILMS //
    private ObservableCollection<FavFilms> userFavoriteFilms = [
        new FavFilms { Id = 1, Title = "Inception", ImageUrl = "JohnWick.jpg" },
        new FavFilms { Id = 2, Title = "The Dark Knight", ImageUrl = "DarkKnight.jpg" },
        new FavFilms { Id = 3, Title = "Interstellar", ImageUrl = "Godfather.jpg" },
        new FavFilms { Id = 4, Title = "The Matrix", ImageUrl = "Avengers.jpg" },
        new FavFilms { Id = 5, Title = "Pulp Fiction", ImageUrl = "Gladiator.jpg" },
        new FavFilms { Id = 6, Title = "Fight Club", ImageUrl = "ForrestGump.jpg" },
        new FavFilms { Id = 7, Title = "The Lord of the Rings", ImageUrl = "Inception.jpg" },
        new FavFilms { Id = 8, Title = "Star Wars", ImageUrl = "Matrix.jpg" },
        new FavFilms { Id = 9, Title = "The Shawshank Redemption", ImageUrl = "PulpFiction.jpg" },
        new FavFilms { Id = 10, Title = "The Godfather", ImageUrl = "Interstellar.jpg" }
        ];
    public ObservableCollection<FavFilms> UserFavoriteFilms
    {
        get { return userFavoriteFilms; }
        set { SetProperty(ref userFavoriteFilms, value); }
    }



    // USER LAST COMMENTS //
    private ObservableCollection<LatestComments> userLatestComments = [ 
        new LatestComments { Id = 1, Comment = "Amazing movie with a mind-bending plot!", MovieTitle = "Inception", CommentDate = DateTime.Now.AddDays(-1), Rating = 5 },
        new LatestComments { Id = 2, Comment = "A thrilling ride from start to finish.", MovieTitle = "The Dark Knight", CommentDate = DateTime.Now.AddDays(-2), Rating = 5 },
        new LatestComments { Id = 3, Comment = "A visually stunning masterpiece.", MovieTitle = "Interstellar", CommentDate = DateTime.Now.AddDays(-3), Rating = 4 },
        new LatestComments { Id = 4, Comment = "A revolutionary sci-fi classic.", MovieTitle = "The Matrix", CommentDate = DateTime.Now.AddDays(-4), Rating = 5 },
        new LatestComments { Id = 5, Comment = "A cult classic with unforgettable characters.", MovieTitle = "Pulp Fiction", CommentDate = DateTime.Now.AddDays(-5), Rating = 5 }

        ];
    public ObservableCollection<LatestComments> UserLatestComments
    {
        get { return userLatestComments; }
        set { SetProperty(ref userLatestComments, value); }
    }





}

public class FavFilms
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}

public class LatestComments
{
    public int Id { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string MovieTitle { get; set; } = string.Empty;
    public DateTime CommentDate { get; set; }
    public int Rating { get; set; }
}