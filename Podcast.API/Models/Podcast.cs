namespace Podcast.API.Models
{
    public class Podcasts
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AudioUrl { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
