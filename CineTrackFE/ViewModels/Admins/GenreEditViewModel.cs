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
            var responseGenres = await _apiService.GetAsync<ICollection<Genre>>("/Api/AdminApi/GetGenres");
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

        if (SelectedGenre == null || SelectedGenre.Id <= 0)
        {
            ErrorMessage = "No genre selected.";
            return;
        }
        if (string.IsNullOrWhiteSpace(SelectedGenre.Name))
        {
            ErrorMessage = "Name cannot be empty.";
            return;
        }

        try
        {
            var response = await _apiService.PutAsync<bool, Genre>("/Api/AdminApi/EditGenre", SelectedGenre.Id, SelectedGenre);
            if (response)
            {
                SelectedGenre = new();
                ErrorMessage = string.Empty;
                IsPopupOpen = false;

                await GetGenresAsync();
            }
            else
            {
                ErrorMessage = "Failed to update genre.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
    }



    // ADD GENRE //
    private async Task AddGenre()
    {
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(SelectedGenre.Name))
        {
            ErrorMessage = "New Genre name is null!";
            return;
        }

        try
        {
            var response = await _apiService.PostAsync<bool, Genre>("/Api/AdminApi/AddGenre", SelectedGenre);
            if (response)
            {
                IsPopupOpen = false;
                await GetGenresAsync();
            }
            else
            {
                ErrorMessage = "Failed to add genre.";
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
            var response = await _apiService.PostAsync<bool, Genre>("/Api/AdminApi/RemoveGenre", SelectedGenre);
            if (response)
            {
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
    private ObservableCollection<Genre> genreList = [
        new Genre { Id = 1, Name = "Action" },
        new Genre { Id = 2, Name = "Comedy" },
        new Genre { Id = 3, Name = "Drama" },
        new Genre { Id = 4, Name = "Horror" },
        new Genre { Id = 5, Name = "Sci-Fi" }
        ];
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
            SetProperty(ref selectedGenre, value);
            if (value != null)
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
