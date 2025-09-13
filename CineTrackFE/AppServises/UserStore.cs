using CineTrackFE.Common.Events;
using CineTrackFE.Models;

namespace CineTrackFE.AppServises;

public interface IUserStore
{
    User? User { get; set; }
}

public class UserStore(IEventAggregator eventAggregator) : IUserStore
{
    private readonly IEventAggregator _eventAggregator = eventAggregator;

    public bool IsUserLogged { get; private set; }

    private User? _user;
    public User? User
    {
        get { return _user; }
        set
        {

            _user = value;
            IsUserLogged = _user != null;

            _eventAggregator.GetEvent<MainViewLoginEvent>().Publish(_user);
        }
    }







}
