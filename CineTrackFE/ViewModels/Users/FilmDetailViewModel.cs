using CineTrackFE.AppServises;
using CineTrackFE.Common;
using CineTrackFE.Common.Events;
using CineTrackFE.Models;
using System.Collections.ObjectModel;

namespace CineTrackFE.ViewModels.Users
{
    public class FilmDetailViewModel : BindableBase, INavigationAware
    {
        private readonly IApiService _apiService;
        private readonly IEventAggregator _eventAggregator;

        private readonly AsyncDelegateCommand<int> GetFilmFromApiAsyncCommand;
        private readonly AsyncDelegateCommand<int> GetCommentsFromApiAsyncCommand;

        public AsyncDelegateCommand ToggleFavoriteCommand { get; }
        public AsyncDelegateCommand SendCommentAsyncCommand { get; }

        public FilmDetailViewModel(IApiService apiService, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _apiService = apiService;
            GetFilmFromApiAsyncCommand = new AsyncDelegateCommand<int>(GetFilmFromApiAsync);
            GetCommentsFromApiAsyncCommand = new AsyncDelegateCommand<int>(GetCommentsFromApiAsync);
            ToggleFavoriteCommand = new AsyncDelegateCommand(ToggleFavoriteAsync);
            SendCommentAsyncCommand = new AsyncDelegateCommand(SendCommentAsync);


            _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Film Details");
        }


        // I-NAVIGATION-AWARE //
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var inputFilmId = navigationContext.Parameters[Const.FilmId]?.ToString();

            // if parametr film-id is int then get film from api
            if (int.TryParse(inputFilmId, out int intFilmId))
            {
                FilmId = intFilmId;
                GetFilmFromApiAsyncCommand.Execute(intFilmId);
                GetCommentsFromApiAsyncCommand.Execute(intFilmId);
            }

            else ErrorMessage = "FilmId parametr is null or wrong format!";
        }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }


        // GET FILM FROM API //
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

        // GET COMMENTS FROM API //
        private async Task GetCommentsFromApiAsync(int id)
        {
            if (id == 0) return;

            try
            {
                var resultComments = await _apiService.GetAsync<ICollection<Comment>>("/api/FilmApi/GetComments", id);
                if (resultComments != null) Comments = new ObservableCollection<Comment>(resultComments);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        // TOGGLE FAVORITE //
        private async Task ToggleFavoriteAsync()
        {
            if (Film == null) return;
            try
            {
                Film.IsMyFavorite = !Film.IsMyFavorite;
                var result = await _apiService.GetAsync<bool>("/api/FilmApi/ToggleFavorite", Film.Id);
                if (result) Film.IsMyFavorite = !Film.IsMyFavorite;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        // SEND COMMENT AND RATING //
        private async Task SendCommentAsync()
        {
            FormErrorMessage = null;

            if (string.IsNullOrWhiteSpace(commentFormText) || string.IsNullOrWhiteSpace(Rating))
            {
                FormErrorMessage = "Chybějící údaje ve formuláři!";
                return;
            }

            if (!int.TryParse(Rating, out int ratingInt) || ratingInt < 0 || ratingInt > 100)
            {
                FormErrorMessage = "Hodnocení může být od 0 do 100%";
                return;
            }

            try
            {
                var IsCommentSend = await _apiService.PostAsync<bool, object>("/api/FilmApi/AddComment", new { FilmId, Text = commentFormText, Rating = ratingInt });
                if (IsCommentSend)
                {
                    Rating = null;
                    CommentFormText = null;

                    await GetCommentsFromApiAsyncCommand.Execute(FilmId);
                }
                else ErrorMessage = "Chyba, komentář nebyl uložen!";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            IsPopupOpen = false;
        }



        // FILM ID PARAMETR //
        private int filmId;
        public int FilmId
        {
            get { return filmId; }
            set { filmId = value; }
        }




        // FILM DETAILS //
        private Film film = new();
        public Film Film
        {
            get { return film; }
            set { SetProperty(ref film, value); }
        }


        // COMMENTS //
        private ObservableCollection<Comment> comments = [];
        public ObservableCollection<Comment> Comments
        {
            get { return comments; }
            set { SetProperty(ref comments, value); }
        }


        // COMMENT TEXT //
        private string? commentFormText;
        public string? CommentFormText
        {
            get { return commentFormText; }
            set { SetProperty(ref commentFormText, value); }
        }


        // RATING //
        private string? rating;
        public string? Rating
        {
            get { return rating; }
            set { SetProperty(ref rating, value); }
        }


        // FORM ERROR MESSAGE //
        private string? formErrorMessage;
        public string? FormErrorMessage
        {
            get { return formErrorMessage; }
            set { SetProperty(ref formErrorMessage, value); }
        }


        // ERROR MESSAGE //
        private string? errorMessage;
        public string? ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }


        // POPUP IS OPEN //

        private bool isPopupOpen;
        public bool IsPopupOpen
        {
            get { return isPopupOpen; }
            set { SetProperty(ref isPopupOpen, value); }
        }










    }
}
