using CineTrackFE.AppServises;
using CineTrackFE.Common;
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
            FormErrorMessage = null;

            if (SelectedUser == null || string.IsNullOrEmpty(SelectedUser.Id))
            {
                FormErrorMessage = "Film is not selected.";
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedUser.Email))
            {
                FormErrorMessage = "Email cannot be empty.";
                return;
            }

            if (SelectedUser.Email.Contains('@') == false || SelectedUser.Email.Contains('.') == false)
            {
                FormErrorMessage = "Email is not valid.";
                return;
            }


            // if one password is not empty
            if (!string.IsNullOrEmpty(SelectedUser?.NewPassword) || !string.IsNullOrEmpty(PasswordConfirmation!))
            {
                if (string.IsNullOrWhiteSpace(SelectedUser?.NewPassword) || string.IsNullOrWhiteSpace(PasswordConfirmation))
                {
                    FormErrorMessage = "Password or Password Confirmation is null or empty!";
                    return;
                }
                if (SelectedUser?.NewPassword != PasswordConfirmation)
                {
                    FormErrorMessage = "Password and Password Confirmation are not the same!";
                    return;
                }

                if (string.IsNullOrWhiteSpace(SelectedUser?.NewPassword) || SelectedUser?.NewPassword.Length < 6)
                {
                    FormErrorMessage = "Password is null or less than 6 characters!";
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(SelectedUser!.UserName))
            {
                SelectedUser.UserName = SelectedUser.Email;
            }

            // Clear roles and add new ones
            SelectedUser.Roles.Clear();
            // Combo box roles checks
            if (SelectedRoleOne != null)
                if (Const.OnlyLettersRegex.IsMatch(SelectedRoleOne)) SelectedUser.Roles.Add(SelectedRoleOne);

            if (SelectedRoleTwo != null && SelectedRoleTwo != SelectedRoleOne)
                if (Const.OnlyLettersRegex.IsMatch(SelectedRoleTwo)) SelectedUser.Roles.Add(SelectedRoleTwo);

            try
            {
                var response = await _apiService.PutAsync<User, User>("/api/UsersApi/EditUser", SelectedUser.Id, SelectedUser);

                if (response != null)
                {
                    var user = UserList.FirstOrDefault(p => p.Id == response.Id);
                    if (user != null)
                    {
                        var userIndex = UserList.IndexOf(user);
                        UserList.Remove(user);
                        UserList.Insert(userIndex, response);
                    }

                    SelectedUser = new();
                    PasswordConfirmation = null;
                    ErrorMessage = string.Empty;
                    IsPopupOpen = false;
                }
                else
                {
                    FormErrorMessage = "Failed to update user.";
                }
            }
            catch (Exception ex)
            {
                FormErrorMessage = $"Error: {ex.Message}";
            }
        }


        // ADD USER //
        private async Task AddUser()
        {
            ErrorMessage = null;

            if (SelectedUser == null)
            {
                FormErrorMessage = "Selected User is null!";
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedUser.Email))
            {
                FormErrorMessage = "Email is null or empty!";
                return;
            }

            if (SelectedUser.Email.Contains('@') == false || SelectedUser.Email.Contains('.') == false)
            {
                FormErrorMessage = "Email is not valid!";
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedUser.NewPassword) || string.IsNullOrWhiteSpace(PasswordConfirmation))
            {
                FormErrorMessage = "Password or Password Confirmation is null or empty!";
                return;
            }

            if (SelectedUser.NewPassword != PasswordConfirmation)
            {
                FormErrorMessage = "Password and Password Confirmation are not the same!";
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedUser.NewPassword) || SelectedUser.NewPassword.Length < 6)
            {
                FormErrorMessage = "Password is null or less than 6 characters!";
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedUser!.UserName))
            {
                SelectedUser.UserName = SelectedUser.Email;
            }

            // ComboBox roles check
            if (SelectedRoleOne != null)
                if (Const.OnlyLettersRegex.IsMatch(SelectedRoleOne)) SelectedUser.Roles.Add(SelectedRoleOne);

            if (SelectedRoleTwo != null && SelectedRoleTwo != SelectedRoleOne)
                if (Const.OnlyLettersRegex.IsMatch(SelectedRoleTwo)) SelectedUser.Roles.Add(SelectedRoleTwo);



            try
            {
                var response = await _apiService.PostAsync<User, User>("/api/UsersApi/AddUser", SelectedUser!);
                if (response != null)
                {
                    PasswordConfirmation = null;
                    IsPopupOpen = false;
                    UserList.Add(response);
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


        // REMOVE USER //
        private async Task RemoveUser()
        {
            ErrorMessage = null;

            if (SelectedUser == null || string.IsNullOrEmpty(SelectedUser.Id))
            {
                ErrorMessage = "Selected User is null or wrong!";
                return;
            }

            try
            {
                var response = await _apiService.DeleteAsync("/api/UsersApi/RemoveUser", SelectedUser.Id);
                if (response)
                {
                    UserList.Remove(userList.First(p => p.Id == SelectedUser.Id));
                    IsPopupOpen = false;
                }
                else
                {
                    ErrorMessage = "Failed to Remove user.";
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
            SelectedUser = new();
            FormErrorMessage = null;
            EditVisibility = Visibility.Collapsed;

            SelectedRoleOne = null;
            SelectedRoleTwo = null;

            IsPopupOpen = true;
        }

        // OPEN EDIT FORM //
        private void OpenEditForm()
        {
            if (selectedUser == null) return;

            FormErrorMessage = null;
            EditVisibility = Visibility.Visible;

            // add selected genres to dropdowns
            if (selectedUser.Roles.Count >= 1)
            {
                var roleOne = RoleList?.FirstOrDefault(p => p == SelectedUser?.Roles.ToList()[0]);
                if (roleOne != null) SelectedRoleOne = roleOne;
            }
            else SelectedRoleOne = null;

            if (selectedUser.Roles.Count >= 2)
            {
                var roleTwo = RoleList?.FirstOrDefault(p => p == SelectedUser?.Roles.ToList()[1]);
                if (roleTwo != null) SelectedRoleTwo = roleTwo;
            }
            else SelectedRoleTwo = null;


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
        private User selectedUser = new();
        public User SelectedUser
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
                    SetProperty(ref selectedUser, new());
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
