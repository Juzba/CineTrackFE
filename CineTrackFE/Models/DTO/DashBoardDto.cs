namespace CineTrackFE.Models.DTO;

# nullable disable
public class DashBoardDto
{
    public List<Film> LatestFilms { get; set; }
    public List<Film> FavoriteFilms { get; set; }
}
