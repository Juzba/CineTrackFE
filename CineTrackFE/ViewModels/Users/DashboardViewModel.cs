using CineTrackFE.AppServises;
using CineTrackFE.Common;
using CineTrackFE.Common.Events;
using CineTrackFE.Models;
using CineTrackFE.Models.DTO;
using CineTrackFE.Views.Users;
using System.Collections.ObjectModel;

namespace CineTrackFE.ViewModels.Users
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private readonly IApiService _apiService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;


        private readonly AsyncDelegateCommand OnInitializeAsyncCommand;
        public DelegateCommand<Film> OpenFilmDetailsCommand { get; }


        public DashboardViewModel(IApiService apiService, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _apiService = apiService;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            OpenFilmDetailsCommand = new DelegateCommand<Film>(film =>
            {
                if (film != null)
                    _regionManager.RequestNavigate(Const.MainRegion, nameof(FilmDetailView), new NavigationParameters() { { Const.FilmId, film.Id } });
            });


            OnInitializeAsyncCommand = new AsyncDelegateCommand(OnInitializeAsync);
            OnInitializeAsyncCommand.Execute();

            _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Dashboard Page");
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
                var dashBoardDto = await _apiService.GetAsync<DashBoardDto>("/api/FilmApi/DashBoardFilms");
                if (dashBoardDto != null)
                {
                    LatestsFilmList = new ObservableCollection<Film>(dashBoardDto.LatestFilms);
                    FavoriteFilms = new ObservableCollection<Film>(dashBoardDto.FavoriteFilms);
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        private ObservableCollection<Film> latestsFilmList = [];
        public ObservableCollection<Film> LatestsFilmList
        {
            get { return latestsFilmList; }
            set { SetProperty(ref latestsFilmList, value); }
        }

        private ObservableCollection<Film> favoriteFilms = [];
        public ObservableCollection<Film> FavoriteFilms
        {
            get { return favoriteFilms; }
            set { SetProperty(ref favoriteFilms, value); }
        }



        private string? errorMessage;
        public string? ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }




    }
}
