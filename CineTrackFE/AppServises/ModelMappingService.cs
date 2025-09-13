using CineTrackFE.Models;

namespace CineTrackFE.AppServises;


public static class ModelMappingService
{

    public static Film CloneFilm(Film source)
    {

        ArgumentNullException.ThrowIfNull(source);

        var clone = new Film
        {
            Id = source.Id,
            Name = source.Name,
            Director = source.Director,
            Description = source.Description,
            ReleaseDate = source.ReleaseDate,
            AvgRating = source.AvgRating,
            ImageFileName = source.ImageFileName,
            IsMyFavorite = source.IsMyFavorite,
            Genres = source.Genres != null ? [.. source.Genres.Select(p => CloneGenre(p))] : []
        };

        return clone;
    }


    public static Genre CloneGenre(Genre source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var clone = new Genre
        {
            Id = source.Id,
            Name = source.Name
        };

        return clone;
    }


    public static User CloneUser(User source)
    {
        ArgumentNullException.ThrowIfNull(source);

        source.Roles ??= [];

        var clone = new User
        {
            Id = source.Id,
            UserName = source.UserName,
            Email = source.Email,
            PhoneNumber = source.PhoneNumber,
            EmailConfirmed = source.EmailConfirmed,
            Roles = [.. source.Roles]


        };
        return clone;
    }




}
