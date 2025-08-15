using CineTrackFE.Common;
using CineTrackFE.Common.Events;
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
        public DelegateCommand NavFilmCommand { get; }



        public MainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            OnInitializeCommand = new DelegateCommand(OnInitialize);
            OnInitializeCommand.Execute();

            _eventAggregator.GetEvent<MainViewTitleEvent>().Subscribe((string titleParametr) => Title = titleParametr);

            NavLoginCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(LoginView)));
            NavRegisterCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(RegisterView)));
            NavDashboardCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(DashboardView)));
            NavFilmCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(FilmView)));
        }



        private void OnInitialize()
        {

        }



        private string title = "";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }






    }
}
