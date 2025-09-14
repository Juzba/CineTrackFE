using CineTrackFE.AppServises;
using CineTrackFE.Models;
using CineTrackFE.Models.DTO;
using System.Collections.ObjectModel;

namespace CineTrackFE.ViewModels.Admins
{
    public class WebStatisticViewModel : BindableBase, INavigationAware
    {

        private readonly IApiService _apiService;

        private readonly AsyncDelegateCommand GetStatisticsAsyncCommand;




        public WebStatisticViewModel(IApiService apiService)
        {
            _apiService = apiService;
            GetStatisticsAsyncCommand = new AsyncDelegateCommand(GetStatisticsAsync);
            GetStatisticsAsyncCommand.Execute();

        }


        // I-NAVIGATION-AWARE //
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;
        public void OnNavigatedTo(NavigationContext navigationContext) { }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }


        public async Task GetStatisticsAsync()
        {
            try
            {
                var response = await _apiService.GetAsync<StatisticsDto>("/api/AdminApi/Statistics");
                if (response != null)
                {
                    Statistics = response;
                    MostPopularFilms = new ObservableCollection<Film>(response.TopMovies.MostPopular);
                    TopRatedFilms = new ObservableCollection<Film>(response.TopMovies.BestRated);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        // STATISTICS //
        private StatisticsDto statistics = new();
        public StatisticsDto Statistics
        {
            get { return statistics; }
            set { SetProperty(ref statistics, value); }
        }

        // MOST POPULAR FILMS //
        private ObservableCollection<Film> mostPopularFilms = [];
        public ObservableCollection<Film> MostPopularFilms
        {
            get { return mostPopularFilms; }
            set { SetProperty(ref mostPopularFilms, value); }
        }

        // TOP RATED FILMS //
        private ObservableCollection<Film> topRatedFilms = [];
        public ObservableCollection<Film> TopRatedFilms
        {
            get { return topRatedFilms; }
            set { SetProperty(ref topRatedFilms, value); }
        }






        // ERROR MESSAGE //
        private string? errorMessage;
        public string? ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }


    }
}
