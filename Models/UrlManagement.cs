namespace UrlShortener.Models
{
    public class UrlManagement
    {
        public int Id { get; set; }
        public string Url { get; set; } = "";
        public string ShorterUrl { get; set; } = "";
    }
}
