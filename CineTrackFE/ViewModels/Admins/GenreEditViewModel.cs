using CineTrackFE.AppServises;
using CineTrackFE.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace CineTrackFE.ViewModels.Admins;

public class GenreEditViewModel : BindableBase, INavigationAware
{
    private readonly IApiService _apiService;

    private readonly AsyncDelegateCommand GetGenresAsyncCommand;

    public DelegateCommand OpenNewFormCommand { get; }
    public AsyncDelegateCommand EditGenreCommand { get; }
    public AsyncDelegateCommand AddGenreCommand { get; }
    public AsyncDelegateCommand RemoveGenreCommand { get; }
    public DelegateCommand ClosePopUpCommand { get; }


    public GenreEditViewModel(IApiService apiService)
    {
        _apiService = apiService;

        GetGenresAsyncCommand = new AsyncDelegateCommand(GetGenresAsync);
        ClosePopUpCommand = new DelegateCommand(() => IsPopupOpen = false);
        EditGenreCommand = new AsyncDelegateCommand(EditGenre);
        RemoveGenreCommand = new AsyncDelegateCommand(RemoveGenre);
        AddGenreCommand = new AsyncDelegateCommand(AddGenre);
        OpenNewFormCommand = new DelegateCommand(OpenNewForm);

        GetGenresAsyncCommand.Execute();
    }


    // I-NAVIGATION-AWARE //
    public bool IsNavigationTarget(NavigationContext navigationContext) => false;
    public void OnNavigatedTo(NavigationContext navigationContext) { }
    public void OnNavigatedFrom(NavigationContext navigationContext) { }



    // GET GENRE LIST FROM DB //
    private async Task GetGenresAsync()
    {
        try
        {
            var responseGenres = await _apiService.GetAsync<ICollection<Genre>>("/api/FilmApi/AllGenres");
            if (responseGenres != null) GenreList = new ObservableCollection<Genre>(responseGenres);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }



    // SEND EDIT GENRE //
    private async Task EditGenre()
    {
        ErrorMessage = null;
        FormErrorMessage = null;

        if (SelectedGenre == null || SelectedGenre.Id <= 0)
        {
            FormErrorMessage = "No genre selected.";
            return;
        }
        if (string.IsNullOrWhiteSpace(SelectedGenre.Name))
        {
            FormErrorMessage = "Name cannot be empty.";
            return;
        }

        if (GenreList.Any(p => p.Name == selectedGenre.Name))
        {
            FormErrorMessage = "Name is same or another genre has this name!";
            return;
        }

        try
        {
            var response = await _apiService.PutAsync<Genre, Genre>("/api/AdminApi/EditGenre", SelectedGenre.Id, SelectedGenre);
            if (response != null)
            {

                var genre = GenreList.FirstOrDefault(p => p.Id == response.Id);
                if (genre != null)
                {
                    genre.Name = response.Name;
                    GenreList = new ObservableCollection<Genre>(GenreList);
                }

                SelectedGenre = new();
                ErrorMessage = string.Empty;
                IsPopupOpen = false;
            }
            else
            {
                FormErrorMessage = "Failed to update genre.";
            }
        }
        catch (Exception ex)
        {
            FormErrorMessage = $"Error: {ex.Message}";
        }
    }



    // ADD GENRE //
    private async Task AddGenre()
    {
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(SelectedGenre.Name))
        {
            FormErrorMessage = "New Genre name is null!";
            return;
        }

        try
        {
            var response = await _apiService.PostAsync<Genre, Genre>("/api/AdminApi/AddGenre", SelectedGenre);
            if (response != null)
            {
                GenreList.Add(response);
                IsPopupOpen = false;
            }

        }
        catch (Exception ex)
        {
            FormErrorMessage = ex.Message;
        }

    }



    // REMOVE GENRE //
    private async Task RemoveGenre()
    {
        ErrorMessage = null;

        if (SelectedGenre == null || SelectedGenre.Id <= 0)
        {
            ErrorMessage = "Selected Genre is null or wrong!";
            return;
        }

        try
        {
            var response = await _apiService.DeleteAsync("/api/AdminApi/RemoveGenre", SelectedGenre.Id);
            if (response)
            {
                var genre = GenreList.FirstOrDefault(g => g.Id == SelectedGenre.Id);
                if (genre != null) GenreList.Remove(genre);

                IsPopupOpen = false;
                await GetGenresAsync();
            }
            else
            {
                ErrorMessage = "Failed to Remove genre.";
            }
        }
        catch (Exception ex)
        {
            FormErrorMessage = ex.Message;
        }
    }


    // OPEN NEW FORM //
    private void OpenNewForm()
    {
        FormErrorMessage = null;
        EditVisibility = Visibility.Collapsed;
        SelectedGenre = new();

        IsPopupOpen = true;
    }

    // OPEN EDIT FORM //
    private void OpenEditForm()
    {
        FormErrorMessage = null;
        EditVisibility = Visibility.Visible;

        IsPopupOpen = true;
    }



    // GENRE LIST //
    private ObservableCollection<Genre> genreList = [];
    public ObservableCollection<Genre> GenreList
    {
        get { return genreList; }
        set { SetProperty(ref genreList, value); }
    }



    // SELECTED GENRE //
    private Genre selectedGenre = new();
    public Genre SelectedGenre
    {
        get { return selectedGenre; }
        set
        {
            var genreCopy = ModelMappingService.CloneGenre(value ?? new Genre());

            SetProperty(ref selectedGenre, genreCopy);
            if (genreCopy.Id > 0)
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
