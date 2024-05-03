namespace JackHenryRedditExercise.Models
{
    public class RedditClientConfiguration
    {
        public const int MaxRequestsPerMinute = 100;

        public string? Secret { get; set; }

        public string? ClientId { get; set; }

        public string? UserAgent { get; set; }

        public string? Subreddit { get; set; }

        public int RequestsPerMinute { get; set; }
    }
}
