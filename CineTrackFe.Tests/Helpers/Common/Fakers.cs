using Bogus;
using CineTrackFE.Models;

namespace CineTrackFE.Tests.Helpers.Common;


public static class Fakers
{
    // GENRE //
    public static readonly Faker<Genre> Genre =
        new Faker<Genre>()
            .UseSeed(1234)
            .RuleFor(g => g.Id, f => f.IndexFaker)
            .RuleFor(g => g.Name, f => $"Genre {f.Random.AlphaNumeric(6)}");

    // FILM //
    public static readonly Faker<Film> Film =
        new Faker<Film>()
            .UseSeed(1234)
            .RuleFor(g => g.Id, f => f.IndexFaker)
            .RuleFor(g => g.Name, f => $"Film {f.Random.AlphaNumeric(6)}")
            .RuleFor(g => g.Director, f => f.Name.FullName())
            .RuleFor(g => g.ReleaseDate, f => f.Date.Between(new DateTime(1970, 1, 1), new DateTime(2025, 1, 1)));

    // USER //
    public static readonly Faker<User> User =
        new Faker<User>()
            .UseSeed(1234)
            .RuleFor(g => g.Id, f => $"Id {f.IndexFaker}")
            .RuleFor(g => g.UserName, f => f.Name.FullName())
            .RuleFor(g => g.Email, f => $"Test Email {f.Random.AlphaNumeric(8)}")
            .RuleFor(g => g.PhoneNumber, f => f.Phone.PhoneNumberFormat(3))
            .RuleFor(g => g.Roles, f => []);


}
