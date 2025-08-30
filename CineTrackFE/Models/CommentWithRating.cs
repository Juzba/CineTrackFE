#nullable disable


using System.ComponentModel.DataAnnotations;

namespace CineTrackFE.Models
{
    public class CommentWithRating
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public string AutorName { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime SendDate { get; set; }
    }
}
