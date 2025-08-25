#nullable disable


namespace CineTrackFE.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string Text { get; set; }
        public int? Rating { get; set; }
        public DateTime SendDate { get; set; }
    }
}
