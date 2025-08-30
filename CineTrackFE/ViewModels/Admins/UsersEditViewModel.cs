using CineTrackFE.AppServises;
using CineTrackFE.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace CineTrackFE.ViewModels.Admins
{
    public class UsersEditViewModel : BindableBase, INavigationAware
    {

        private readonly IApiService _apiService;

        private readonly AsyncDelegateCommand GetUsersAsyncCommand;

        public DelegateCommand OpenNewFormCommand { get; }
        public AsyncDelegateCommand EditUserCommand { get; }
        public AsyncDelegateCommand AddUserCommand { get; }
        public AsyncDelegateCommand RemoveUserCommand { get; }
        public DelegateCommand ClosePopUpCommand { get; }

        public UsersEditViewModel(IApiService apiService)
        {
            _apiService = apiService;

            GetUsersAsyncCommand = new AsyncDelegateCommand(GetUsersAsync);
            ClosePopUpCommand = new DelegateCommand(() => IsPopupOpen = false);
            EditUserCommand = new AsyncDelegateCommand(EditUser);
            RemoveUserCommand = new AsyncDelegateCommand(RemoveUser);
            AddUserCommand = new AsyncDelegateCommand(AddUser);
            OpenNewFormCommand = new DelegateCommand(OpenNewForm);

            GetUsersAsyncCommand.Execute();
        }


        // I-NAVIGATION-AWARE //
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;
        public void OnNavigatedTo(NavigationContext navigationContext) { }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }



        // GET USER LIST FROM DB //
        private async Task GetUsersAsync()
        {

            try
            {
                var response = await _apiService.GetAsync<ICollection<User>>("/api/UsersApi/AllUsers");
                if (response != null) UserList = new ObservableCollection<User>(response);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        // EDIT USER //
        private async Task EditUser()
        {
            ErrorMessage = null;

            //if (SelectedFilm == null || SelectedFilm.Id <= 0)
            //{
            //    ErrorMessage = "Film is not selected.";
            //    return;
            //}
            //if (string.IsNullOrWhiteSpace(SelectedFilm.Name))
            //{
            //    ErrorMessage = "Name cannot be empty.";
            //    return;
            //}

            //if (string.IsNullOrWhiteSpace(SelectedFilm.Director))
            //{
            //    FormErrorMessage = "New Director name is null!";
            //    return;
            //}

            //if (!(SelectedRoleOne?.Id > 0 || SelectedRoleTwo?.Id > 0 || SelectedGenreThree?.Id > 0))
            //{
            //    FormErrorMessage = "Genres count must be between 1 - 3";
            //    return;
            //}

            //// filter genres -> if they are same or null
            //GenresFromComboboxToSelectedFilm();

            //try
            //{
            //    var response = await _apiService.PutAsync<Film, Film>("/api/AdminApi/EditFilm", SelectedFilm.Id, SelectedFilm);

            //    if (response != null)
            //    {
            //        var film = UserList.FirstOrDefault(p => p.Id == response.Id);
            //        if (film != null)
            //        {
            //            UserList.Remove(film);
            //            UserList.Add(response);
            //            GenreList = new ObservableCollection<Genre>(GenreList);
            //        }

            //        SelectedFilm = new();
            //        ErrorMessage = string.Empty;
            //        IsPopupOpen = false;
            //    }
            //    else
            //    {
            //        FormErrorMessage = "Failed to update film.";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    FormErrorMessage = $"Error: {ex.Message}";
            //}
        }


        // ADD USER //
        private async Task AddUser()
        {
            ErrorMessage = null;
            

            //if (string.IsNullOrWhiteSpace(SelectedFilm.Name))
            //{
            //    FormErrorMessage = "New Genre name is null!";
            //    return;
            //}

            //if (string.IsNullOrWhiteSpace(SelectedFilm.Director))
            //{
            //    FormErrorMessage = "New Director name is null!";
            //    return;
            //}

            //if (!(SelectedRoleOne?.Id > 0 || SelectedRoleTwo?.Id > 0 || SelectedGenreThree?.Id > 0))
            //{
            //    FormErrorMessage = "Genres count must be between 1 - 3";
            //    return;
            //}

            //// filter genres -> if they are same or null
            //GenresFromComboboxToSelectedFilm();

            //try
            //{
            //    var response = await _apiService.PostAsync<Film, Film>("/api/AdminApi/AddFilm", SelectedFilm);
            //    if (response != null)
            //    {
            //        IsPopupOpen = false;
            //        UserList.Add(response);
            //    }
            //    else
            //    {
            //        ErrorMessage = "Failed to add film.";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    FormErrorMessage = ex.Message;
            //}

        }


        // REMOVE USER //
        private async Task RemoveUser()
        {
            ErrorMessage = null;

            //if (SelectedFilm == null || SelectedFilm.Id <= 0)
            //{
            //    ErrorMessage = "Selected film is null or wrong!";
            //    return;
            //}

            //try
            //{
            //    var response = await _apiService.DeleteAsync("/api/AdminApi/RemoveFilm", SelectedFilm.Id);
            //    if (response)
            //    {
            //        IsPopupOpen = false;
            //        UserList.Remove(userList.First(p => p.Id == selectedUser.Id));
            //    }
            //    else
            //    {
            //        ErrorMessage = "Failed to Remove film.";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    FormErrorMessage = ex.Message;
            //}
        }


        // OPEN NEW FORM //
        private async void OpenNewForm()
        {
            SelectedUser = null;
            FormErrorMessage = null;
            EditVisibility = Visibility.Collapsed;

            SelectedRoleOne = null;
            SelectedRoleTwo = null;

            IsPopupOpen = true;
        }

        // OPEN EDIT FORM //
        private async void OpenEditForm()
        {
            if (selectedUser == null) return;

            //if (GenreList == null) await GetGenreAsync();

            FormErrorMessage = null;
            EditVisibility = Visibility.Visible;


            //if (selectedUser.Genres.Count >= 1)
            //{
            //    var genreOne = UserList?.FirstOrDefault(p => p.Id == SelectedUser.Roles[0]);
            //    if (genreOne != null) SelectedRoleOne = genreOne;
            //}
            //else SelectedRoleOne = null;

            //if (selectedUser.Genres.Count >= 2)
            //{
            //    var genreTwo = GenreList?.FirstOrDefault(p => p.Id == SelectedFilm.Genres[1].Id);
            //    if (genreTwo != null) SelectedRoleTwo = genreTwo;
            //}
            //else SelectedRoleTwo = null;


            IsPopupOpen = true;
        }



        // USER LIST //
        private ObservableCollection<User> userList = [];
        public ObservableCollection<User> UserList
        {
            get { return userList; }
            set { SetProperty(ref userList, value); }
        }

        // ROLE LIST //
        private ObservableCollection<string> roleList = ["User", "Admin", "----"];
        public ObservableCollection<string> RoleList
        {
            get { return roleList; }
            set { SetProperty(ref roleList, value); }
        }



        // SELECTED USER //
        private User? selectedUser;
        public User? SelectedUser
        {
            get { return selectedUser; }
            set
            {
                if (value != null)
                {
                    var cloneUser = ModelMappingService.CloneUser(value);

                    SetProperty(ref selectedUser, cloneUser);
                    if (!string.IsNullOrWhiteSpace(value.Id))
                        OpenEditForm();
                }
                else
                {
                    SetProperty(ref selectedUser, value);
                }
            }
        }



        // SELECTED ROLE ONE //
        private string? selectedRoleOne;
        public string? SelectedRoleOne
        {
            get { return selectedRoleOne; }
            set { SetProperty(ref selectedRoleOne, value); }
        }

        // SELECTED ROLE TWO //
        private string? selectedRoleTwo;
        public string? SelectedRoleTwo
        {
            get { return selectedRoleTwo; }
            set { SetProperty(ref selectedRoleTwo, value); }
        }

        // PASSWORD CONFIRMATION //
        private string? passwordConfirmation;
        public string? PasswordConfirmation
        {
            get { return passwordConfirmation; }
            set { SetProperty(ref passwordConfirmation, value); }
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
