namespace JackHenryRedditExercise.Models
{
    using System.Text.Json.Serialization;

    public class SubRedditData
    {
        [JsonPropertyName("after}")]
        public string? After { get; set; }

        [JsonPropertyName("dist")]
        public int? Distribution { get; set; }

        [JsonPropertyName("modhash")]
        public string? ModHash { get; set; }

        [JsonPropertyName("geo_filter")]
        public string? GeoFilter { get; set; }

        [JsonPropertyName("before")]
        public string? Before { get; set; }

        [JsonPropertyName("children")]
        public ICollection<T3Listing> Children { get; set; } = new List<T3Listing>();
    }
}
