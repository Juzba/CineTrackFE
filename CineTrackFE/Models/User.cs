#nullable disable

namespace CineTrackFE.Models;

public class User
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string NewPassword { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool EmailConfirmed { get; set; }
    public ICollection<string> Roles { get; set; }


}
