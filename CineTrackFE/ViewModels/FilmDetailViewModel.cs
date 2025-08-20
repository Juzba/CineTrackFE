using CineTrackFE.AppServises;
using CineTrackFE.Common;
using CineTrackFE.Common.Events;
using CineTrackFE.Models;

namespace CineTrackFE.ViewModels
{
    public class FilmDetailViewModel : BindableBase, INavigationAware
    {
        private readonly IApiService _apiService;
        private readonly IEventAggregator _eventAggregator;

        private readonly AsyncDelegateCommand<int> GetFilmFromApiAsyncCommand;

        public FilmDetailViewModel(IApiService apiService, IEventAggregator eventAggregator)
        {
            _apiService = apiService;
            GetFilmFromApiAsyncCommand = new AsyncDelegateCommand<int>(GetFilmFromApiAsync);
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Film Details");
        }




        // I-NAVIGATION-AWARE //
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;
        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            var inputFilmId = navigationContext.Parameters[Const.FilmId]?.ToString();

            if (int.TryParse(inputFilmId, out int intFilmId))
            {
                GetFilmFromApiAsyncCommand.Execute(intFilmId);
            }
            else
            {
                ErrorMessage = "FilmId parametr is null or wrong format!"; // Error handling if FilmId is not valid
            }

        }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }



        private async Task GetFilmFromApiAsync(int id)
        {
            if (id == 0) return;

            try
            {
                var result = await _apiService.GetAsync<Film>("/api/FilmApi/FilmDetails", id);
                if (result != null) Film = result;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }



        // FILM DETAILS //
        private Film film = new();
        public Film Film
        {
            get { return film; }
            set { SetProperty(ref film, value); }
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
