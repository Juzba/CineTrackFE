using CineTrackFE.AppServises;
using CineTrackFE.Common;
using CineTrackFE.Common.Events;
using CineTrackFE.Models;
using CineTrackFE.Views.Users;
using CineTrackFE.Views.Admin;
using CineTrackFE.Views.Login;
using System.Windows;

namespace CineTrackFE.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAuthService _authService;



        public DelegateCommand NavLoginCommand { get; }
        public DelegateCommand NavRegisterCommand { get; }
        public DelegateCommand NavDashboardCommand { get; }
        public DelegateCommand NavCatalogCommand { get; }
        public DelegateCommand NavUserProfilCommand { get; }
        public DelegateCommand NavMainAdminCommand { get; }
        public DelegateCommand LogoutCommand { get; }



        public MainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IAuthService authService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _authService = authService;

            _eventAggregator.GetEvent<MainViewTitleEvent>().Subscribe((string titleParametr) => Title = titleParametr);
            _eventAggregator.GetEvent<MainViewLoginEvent>().Subscribe((User? user) => Login(user));

            NavLoginCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(LoginView)));
            NavRegisterCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(RegisterView)));
            NavDashboardCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(DashboardView)));
            NavCatalogCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(CatalogView)));
            NavUserProfilCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(UserProfilView)));
            NavMainAdminCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(MainAdminView)));
            LogoutCommand = new DelegateCommand(Logout);


        }



        private void Login(User? user)
        {
            if (user == null) return;

            UserName = user.UserName;
            IsVisible = false;

            if (user.Roles.Contains("Admin"))
            {
                Role = "Admin";
                AdminVisibility = Visibility.Visible;
            }
            else
            {
                Role = "User";
            }


        }

        private void Logout()
        {
            UserName = null;
            Role = null;
            AdminVisibility = Visibility.Collapsed;
            IsVisible = true;

            _authService.Logout();
            _regionManager.RequestNavigate(Const.MainRegion, nameof(LoginView));
        }




        // TITLE //
        private string title = "";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }


        // USERNAME //
        private string? userName;
        public string? UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        // ROLE //
        private string? role;
        public string? Role
        {
            get { return role; }
            set { SetProperty(ref role, value); }
        }


        // LOGIN VISIBILITY //
        private bool isVisible = true;
        public bool IsVisible
        {
            get { return isVisible; }
            set { SetProperty(ref isVisible, value); }
        }

        // ADMIN VISIBILITY //
        private Visibility adminVisibility = Visibility.Collapsed;
        public Visibility AdminVisibility
        {
            get { return adminVisibility; }
            set { SetProperty(ref adminVisibility, value); }
        }



    }
}
