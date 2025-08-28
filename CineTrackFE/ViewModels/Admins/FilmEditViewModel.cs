using CineTrackFE.AppServises;
using CineTrackFE.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace CineTrackFE.ViewModels.Admins
{
    public class FilmEditViewModel : BindableBase, INavigationAware
    {

        private readonly IApiService _apiService;

        private readonly AsyncDelegateCommand GetFilmsAsyncCommand;

        public DelegateCommand OpenNewFormCommand { get; }
        public AsyncDelegateCommand EditFilmCommand { get; }
        public AsyncDelegateCommand AddFilmCommand { get; }
        public AsyncDelegateCommand RemoveFilmCommand { get; }
        public DelegateCommand ClosePopUpCommand { get; }


        public FilmEditViewModel(IApiService apiService)
        {
            _apiService = apiService;

            GetFilmsAsyncCommand = new AsyncDelegateCommand(GetFilmAsync);
            ClosePopUpCommand = new DelegateCommand(() => IsPopupOpen = false);
            EditFilmCommand = new AsyncDelegateCommand(EditFilm);
            RemoveFilmCommand = new AsyncDelegateCommand(RemoveFilm);
            AddFilmCommand = new AsyncDelegateCommand(AddFilm);
            OpenNewFormCommand = new DelegateCommand(OpenNewForm);

            GetFilmsAsyncCommand.Execute();
        }


        // I-NAVIGATION-AWARE //
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;
        public void OnNavigatedTo(NavigationContext navigationContext) { }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }



        // GET FILM LIST FROM DB //
        private async Task GetFilmAsync()
        {
            try
            {
                var response = await _apiService.GetAsync<ICollection<Film>>("/api/AdminApi/AllFilms");
                if (response != null) FilmList = new ObservableCollection<Film>(response);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        // GET GENRE LIST FROM DB //
        private async Task GetGenreAsync()
        {
            try
            {
                var response = await _apiService.GetAsync<ICollection<Genre>>("/api/FilmApi/AllGenres");
                if (response != null) GenreList = new ObservableCollection<Genre>(response);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }



        // EDIT FILM //
        private async Task EditFilm()
        {
            ErrorMessage = null;

            if (SelectedFilm == null || SelectedFilm.Id <= 0)
            {
                ErrorMessage = "Film is not selected.";
                return;
            }
            if (string.IsNullOrWhiteSpace(SelectedFilm.Name))
            {
                ErrorMessage = "Name cannot be empty.";
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedFilm.Director))
            {
                FormErrorMessage = "New Director name is null!";
                return;
            }

            if (SelectedFilm.Genres.Count <= 0 || SelectedFilm.Genres.Count > 3)
            {
                FormErrorMessage = "Genres count must be between 1 - 3";
                return;
            }

            try
            {
                var response = await _apiService.PutAsync<bool, Film>("/api/AdminApi/EditGenre", SelectedFilm.Id, SelectedFilm);
                if (response)
                {
                    SelectedFilm = new();
                    ErrorMessage = string.Empty;
                    IsPopupOpen = false;

                    await GetFilmAsync();
                }
                else
                {
                    FormErrorMessage = "Failed to update film.";
                }
            }
            catch (Exception ex)
            {
                FormErrorMessage = $"Error: {ex.Message}";
            }
        }



        // ADD FILM //
        private async Task AddFilm()
        {
            ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(SelectedFilm.Name))
            {
                FormErrorMessage = "New Genre name is null!";
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedFilm.Director))
            {
                FormErrorMessage = "New Director name is null!";
                return;
            }

            if (SelectedFilm.Genres.Count <= 0 || SelectedFilm.Genres.Count > 3)
            {
                FormErrorMessage = "Genres count must be between 1 - 3";
                return;
            }

            try
            {
                var response = await _apiService.PostAsync<bool, Film>("/api/AdminApi/AddFilm", SelectedFilm);
                if (response)
                {
                    IsPopupOpen = false;
                    await GetFilmAsync();
                }
                else
                {
                    ErrorMessage = "Failed to add film.";
                }
            }
            catch (Exception ex)
            {
                FormErrorMessage = ex.Message;
            }

        }



        // REMOVE FILM //
        private async Task RemoveFilm()
        {
            ErrorMessage = null;

            if (SelectedFilm == null || SelectedFilm.Id <= 0)
            {
                ErrorMessage = "Selected film is null or wrong!";
                return;
            }

            try
            {
                var response = await _apiService.DeleteAsync<bool>("/api/AdminApi/RemoveGenre", SelectedFilm.Id);
                if (response)
                {
                    IsPopupOpen = false;
                    await GetFilmAsync();
                }
                else
                {
                    ErrorMessage = "Failed to Remove film.";
                }
            }
            catch (Exception ex)
            {
                FormErrorMessage = ex.Message;
            }
        }


        // OPEN NEW FORM //
        private async void OpenNewForm()
        {
            if (GenreList == null) await GetGenreAsync();

            FormErrorMessage = null;
            EditVisibility = Visibility.Collapsed;
            SelectedFilm = new();

            IsPopupOpen = true;
        }

        // OPEN EDIT FORM //
        private async void OpenEditForm()
        {
            if (GenreList == null) await GetGenreAsync();

            FormErrorMessage = null;
            EditVisibility = Visibility.Visible;

            IsPopupOpen = true;
        }



        // FILM LIST //
        private ObservableCollection<Film> filmList = [];
        public ObservableCollection<Film> FilmList
        {
            get { return filmList; }
            set { SetProperty(ref filmList, value); }
        }

        // GENRE LIST //
        private ObservableCollection<Genre> genreList = null!;
        public ObservableCollection<Genre> GenreList
        {
            get { return genreList; }
            set { SetProperty(ref genreList, value); }
        }



        // SELECTED FILM //
        private Film selectedFilm = new();
        public Film SelectedFilm
        {
            get { return selectedFilm; }
            set
            {
                SetProperty(ref selectedFilm, value);
                if (value != null && value.Id > 0)
                    OpenEditForm();
            }
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




        // POPUP OPEN //
        private bool isPopupOpen;
        public bool IsPopupOpen
        {
            get { return isPopupOpen; }
            set { SetProperty(ref isPopupOpen, value); }
        }


        // EDIT VISIBILITY //
        private Visibility editVisibility = Visibility.Collapsed;
        public Visibility EditVisibility
        {
            get { return editVisibility; }
            set { SetProperty(ref editVisibility, value); }
        }

    }
}
