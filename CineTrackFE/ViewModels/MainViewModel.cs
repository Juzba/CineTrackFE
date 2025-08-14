using CineTrackFE.Common;
using CineTrackFE.Views;

namespace CineTrackFE.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;


        private readonly DelegateCommand OnInitializeCommand;

        public DelegateCommand NavLoginCommand { get; }
        public DelegateCommand NavRegisterCommand { get; }
        public DelegateCommand NavHomeCommand { get; }



        public MainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            OnInitializeCommand = new DelegateCommand(OnInitialize);
            OnInitializeCommand.Execute();

            NavLoginCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(LoginView)));
            NavRegisterCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(RegisterView)));
            NavHomeCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.MainRegion, nameof(HomeView)));
        }

        private void OnInitialize()
        {

        }


    }
}
