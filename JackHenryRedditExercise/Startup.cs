namespace JackHenryRedditExercise
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using JackHenryRedditExercise.Interfaces;
    using JackHenryRedditExercise.Models;
    using JackHenryRedditExercise.Services;

    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }

        public Startup()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            Configuration = configurationBuilder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.Configure<RedditClientConfiguration>(Configuration.GetSection(nameof(RedditClientConfiguration)));
            services.AddSingleton<IRedditClient, RedditClientService>();
            services.AddSingleton<IPostService, RedditPostService>();
        }
    }
}