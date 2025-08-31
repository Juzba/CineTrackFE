using CineTrackFE.Common;
using CineTrackFE.Common.Events;
using CineTrackFE.Views.Admins;

namespace CineTrackFE.ViewModels.Admins
{
    public class MainAdminViewModel : BindableBase, IRegionAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAgregator;

        public DelegateCommand NavUsersEditCommand { get; }
        public DelegateCommand NavFilmEditCommand { get; }
        public DelegateCommand NavGenreEditCommand { get; }
        public DelegateCommand NavWebStatisticCommand { get; }

        public MainAdminViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAgregator = eventAggregator;

            _eventAgregator.GetEvent<MainViewTitleEvent>().Publish("Admin Page");

            NavUsersEditCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.AdminRegion, nameof(UsersEditView)));
            NavFilmEditCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.AdminRegion, nameof(FilmEditView)));
            NavGenreEditCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.AdminRegion, nameof(GenreEditView)));
            NavWebStatisticCommand = new DelegateCommand(() => _regionManager.RequestNavigate(Const.AdminRegion, nameof(WebStatisticView)));
        }


        // I-NAVIGATION-AWARE //
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;
        public void OnNavigatedTo(NavigationContext navigationContext) { _regionManager.RequestNavigate(Const.AdminRegion, nameof(FilmEditView)); }
        public void OnNavigatedFrom(NavigationContext navigationContext) { _regionManager.Regions.Remove(Const.AdminRegion); }






    }
}
