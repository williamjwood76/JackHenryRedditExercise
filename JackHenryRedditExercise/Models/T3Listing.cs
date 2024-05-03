namespace JackHenryRedditExercise.Models
{
    using System.Text.Json.Serialization;

    public class T3Listing
    {
        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("data")]
        public T3Data? Data { get; set; }
    }
}
