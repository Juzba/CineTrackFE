using CineTrackFE.Common;
using CineTrackFE.Common.Events;
using CineTrackFE.Models;
using CineTrackFE.Views;

namespace CineTrackFE.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;


        private readonly DelegateCommand OnInitializeCommand;

        public DelegateCommand NavLoginCommand { get; }
        public DelegateCommand NavRegisterCommand { get; }
        public DelegateCommand NavDashboardCommand { get; }
        public DelegateCommand NavCatalogCommand { get; }



        public MainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            OnInitializeCommand = new DelegateCommand(OnInitialize);
            OnInitializeCommand.Execute();

            _eventAggregator.GetEvent<MainViewTitleEvent>().Subscribe((string titleParametr) => Title = titleParametr);
            _eventAggregator.GetEvent<MainViewLoginEvent>().Subscribe((User? user) => Login(user));

            NavLoginCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(LoginView)));
            NavRegisterCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(RegisterView)));
            NavDashboardCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(DashboardView)));
            NavCatalogCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(CatalogView)));
        }



        private void OnInitialize()
        {

        }
        private void Login(User? user)
        {
            // login logika
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





    }
}
