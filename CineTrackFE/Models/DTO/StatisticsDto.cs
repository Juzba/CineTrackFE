namespace CineTrackFE.Models.DTO;

public class StatisticsDto
{
    public Overview Overview { get; set; } = new();
    public TopMovies TopMovies { get; set; } = new();
    public UserActivity UserActivity { get; set; } = new();

}

public class Overview
{
    public int TotalMovies { get; set; }
    public int TotalUsers { get; set; }
    public int TotalRatings { get; set; }
    public double AverageRating { get; set; }
    public int TotalComments { get; set; }

}

public class TopMovies
{
    public List<Film> BestRated { get; set; } = [];
    public List<Film> MostPopular { get; set; } = [];
    public List<Film> Newest { get; set; } = [];
}


public class UserActivity
{
    public double AverageCommentsPerUser { get; set; }
    public List<User> MostActiveUsers { get; set; } = [];

}