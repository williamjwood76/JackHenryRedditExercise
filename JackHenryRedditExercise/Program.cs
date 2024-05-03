namespace JackHenryRedditExercise
{
    using JackHenryRedditExercise.Interfaces;
    using JackHenryRedditExercise.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        private static int Interval;
        private static IRedditClient? client;
        private static IPostService? postService;

        public async static Task Main(string[] args)
        {
            IConfigurationSection? configuration;
            var applicationBuilder = Host.CreateApplicationBuilder(args);
            var startup = new Startup();

            startup.ConfigureServices(applicationBuilder.Services);

            applicationBuilder.Logging.ClearProviders();
            applicationBuilder.Logging.AddConsole();
            applicationBuilder.Logging.SetMinimumLevel(LogLevel.Information);
            applicationBuilder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);

            var host = applicationBuilder.Build();
            configuration = startup.Configuration.GetSection(nameof(RedditClientConfiguration));

            Interval = int.Parse(configuration["Interval"] ?? "15000");

            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                client = provider.GetRequiredService<IRedditClient>();
                postService = provider.GetRequiredService<IPostService>();
            }

            while(true)
            {
                try
                {
                    var details = await client.GetDetails();
                    Console.Clear();

                    if (details != null)
                    {
                        Console.WriteLine(postService.TopUpvotedPosts(details));
                    }

                    var authors = await client.GetTopAuthors();

                    if (authors != null)
                    {
                        Console.WriteLine(postService.AuthorsWithMostPosts(authors));
                    }

                    Thread.Sleep(Interval);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error was encountered: {ex}");
                }
            }
        }
    }
}