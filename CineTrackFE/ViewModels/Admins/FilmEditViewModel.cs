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

            if (SelectedGenreOne == null && SelectedGenreTwo == null && SelectedGenreThree == null)
            {
                FormErrorMessage = "Genres count must be between 1 - 3";
                return;
            }

            SelectedFilm.Genres = [];
            if (SelectedGenreOne != null) SelectedFilm.Genres.Add(SelectedGenreOne);
            if (SelectedGenreTwo != null) SelectedFilm.Genres.Add(SelectedGenreTwo);
            if (SelectedGenreThree != null) SelectedFilm.Genres.Add(SelectedGenreThree);


            try
            {
                var response = await _apiService.PutAsync<Film, Film>("/api/AdminApi/EditFilm", SelectedFilm.Id, SelectedFilm);

                if (response != null)
                {
                    var film = FilmList.FirstOrDefault(p => p.Id == response.Id);
                    if (film != null)
                    {
                        FilmList.Remove(film);
                        FilmList.Add(response);
                        GenreList = new ObservableCollection<Genre>(GenreList);
                    }

                    SelectedFilm = new();
                    ErrorMessage = string.Empty;
                    IsPopupOpen = false;
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

            if (SelectedGenreOne == null && SelectedGenreTwo == null && SelectedGenreThree == null)
            {
                FormErrorMessage = "Genres count must be between 1 - 3";
                return;
            }

            // filter genres -> if they are same or null
            SelectedFilm.Genres = [];
            if (SelectedGenreOne != null) SelectedFilm.Genres.Add(SelectedGenreOne);
            if (SelectedGenreTwo != null && !SelectedFilm.Genres.Contains(selectedGenreTwo)) SelectedFilm.Genres.Add(SelectedGenreTwo);
            if (SelectedGenreThree != null && !SelectedFilm.Genres.Contains(selectedGenreThree)) SelectedFilm.Genres.Add(SelectedGenreThree);

            try
            {
                var response = await _apiService.PostAsync<Film, Film>("/api/AdminApi/AddFilm", SelectedFilm);
                if (response != null)
                {
                    IsPopupOpen = false;
                    FilmList.Add(response);
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
                var response = await _apiService.DeleteAsync("/api/AdminApi/RemoveFilm", SelectedFilm.Id);
                if (response)
                {
                    IsPopupOpen = false;
                    FilmList.Remove(filmList.First(p => p.Id == selectedFilm.Id));
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
            SelectedFilm = new()
            {
                ReleaseDate = DateTime.Now
            };

            SelectedGenreOne = null;
            SelectedGenreTwo = null;
            SelectedGenreThree = null;

            IsPopupOpen = true;
        }

        // OPEN EDIT FORM //
        private async void OpenEditForm()
        {
            if (selectedFilm == null) return;

            if (GenreList == null) await GetGenreAsync();

            FormErrorMessage = null;
            EditVisibility = Visibility.Visible;


            if (selectedFilm.Genres.Count >= 1)
            {
                var genreOne = GenreList?.FirstOrDefault(p => p.Id == SelectedFilm.Genres[0].Id);
                if (genreOne != null) SelectedGenreOne = genreOne;
            }
            else SelectedGenreOne = null;

            if (selectedFilm.Genres.Count >= 2)
            {
                var genreTwo = GenreList?.FirstOrDefault(p => p.Id == SelectedFilm.Genres[1].Id);
                if (genreTwo != null) SelectedGenreTwo = genreTwo;
            }
            else SelectedGenreTwo = null;


            if (selectedFilm.Genres.Count >= 3)
            {
                var genreThree = GenreList?.FirstOrDefault(p => p.Id == SelectedFilm.Genres[2].Id);
                if (genreThree != null) SelectedGenreThree = genreThree;
            }
            else SelectedGenreThree = null;


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
                if (value != null)
                {

                    var filmCopy = ModelMappingService.CloneFilm(value);

                    SetProperty(ref selectedFilm, filmCopy);
                    if (value != null && value.Id > 0)
                        OpenEditForm();
                }
            }
        }



        // SELECTED GENRE ONE //
        private Genre? selectedGenreOne;
        public Genre? SelectedGenreOne
        {
            get { return selectedGenreOne; }
            set { SetProperty(ref selectedGenreOne, value); }
        }

        // SELECTED GENRE TWO //
        private Genre? selectedGenreTwo;
        public Genre? SelectedGenreTwo
        {
            get { return selectedGenreTwo; }
            set { SetProperty(ref selectedGenreTwo, value); }
        }

        // SELECTED GENRE THREE //
        private Genre? selectedGenreThree;
        public Genre? SelectedGenreThree
        {
            get { return selectedGenreThree; }
            set { SetProperty(ref selectedGenreThree, value); }
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
