﻿namespace CineTrackFE.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int UserRating { get; set; }



        // Film //
        public int FilmId { get; set; }
        public Film Film { get; set; } = null!;


        // User //
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

    }
}
