namespace JackHenryRedditExercise.Services
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using JackHenryRedditExercise.Models;
    using JackHenryRedditExercise.Interfaces;

    public class RedditClientService : IRedditClient
    {
        public RedditClientService(
            IHttpClientFactory httpClientFactory,
            IOptions<RedditClientConfiguration> configuration)
        {
            HttpClientFactory = httpClientFactory;
            Configuration = configuration.Value;
        }

        private IHttpClientFactory HttpClientFactory { get; set; }

        public RedditClientConfiguration Configuration { get; private set; }

        public async Task<SubRedditDetails?> GetDetails(string? subReddit = null, int? takeCount = 5)
        {
            using (var httpClient = HttpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", Configuration.UserAgent);

                var apiUrl = $"https://www.reddit.com/r/{subReddit ?? Configuration.Subreddit}/top.json?limit={takeCount}";

                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<SubRedditDetails>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new HttpRequestException(response.ToString());
                }
            }
        }

        public async Task<IEnumerable<AuthorDetails>> GetTopAuthors(string? subReddit = null, int? takeCount = 5)
        {
            using (var httpClient = HttpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", Configuration.UserAgent);

                var url = $"https://www.reddit.com/r/{subReddit ?? Configuration.Subreddit}/new.json?limit=100";

                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var details = JsonSerializer.Deserialize<SubRedditDetails>(await response.Content.ReadAsStringAsync());

                    if (details != null)
                    {
                        return CountAuthorsPosts(details, takeCount.GetValueOrDefault());
                    }
                }
                else
                {
                    throw new HttpRequestException(response.ToString());
                }

                return new List<AuthorDetails>();
            }
        }

        private static IEnumerable<AuthorDetails> CountAuthorsPosts(SubRedditDetails details, int takeCount)
        {
            var dictionary = new Dictionary<string, AuthorDetails>();

            foreach (var listing in details.Data.Children)
            {
                if (listing.Data?.Author != null)
                {
                    if (dictionary.ContainsKey(listing.Data.Author))
                    {
                        dictionary[listing.Data.Author].PostCount++;
                    }
                    else
                    {
                        dictionary.Add(listing.Data.Author,
                                       new AuthorDetails
                                       {
                                           Name = listing.Data?.Author ?? string.Empty,
                                           PostCount = 1
                                       });
                    }
                }
            }

            return dictionary.Values.OrderByDescending(a => a.PostCount).ToList().Take(takeCount);
        }
    }
}