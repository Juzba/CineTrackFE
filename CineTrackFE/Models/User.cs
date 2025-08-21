#nullable disable

namespace CineTrackFE.Models;

public class User
{
    public string UserName { get; set; }
    public string Email { get; set; }

    public IList<string> Roles { get; set; }


}
