using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CineTrackFE.Models;

public class User 
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string EmailConfirmed { get; set; }
    public string Role { get; set; }

    public ICollection<Comment> Comments { get; set; } = [];


}
