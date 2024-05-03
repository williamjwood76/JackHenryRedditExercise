namespace JackHenryRedditExercise.Models
{
    using System.Text.Json.Serialization;

    public class SubRedditDetails
    {
        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("data")]
        public SubRedditData Data { get; set; } = new SubRedditData();
    }
}
