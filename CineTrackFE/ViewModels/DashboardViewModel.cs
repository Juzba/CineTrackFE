using CineTrackFE.AppServises;
using CineTrackFE.Common.Events;
using CineTrackFE.Models;
using System.Collections.ObjectModel;
using System.Numerics;

namespace CineTrackFE.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private readonly IApiService _apiService;
        private readonly IEventAggregator _eventAggregator;
        

        private readonly AsyncDelegateCommand OnInitializeAsyncCommand;


        public DashboardViewModel(IApiService apiService, IEventAggregator eventAggregator)
        {
            _apiService = apiService;
            _eventAggregator = eventAggregator;
            

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
                var filmListDb = await _apiService.GetAsync<IEnumerable<Film>>("/api/FilmApi/LatestFilms");
                if (filmListDb != null) FilmList = new ObservableCollection<Film>(filmListDb);

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                // chyba
            }
                



        }


        private ObservableCollection<Film> filmList = [];
        public ObservableCollection<Film> FilmList
        {
            get { return filmList; }
            set { SetProperty(ref filmList, value); }
        }



        private string? errorMessage;
        public string? ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }




    }
}
