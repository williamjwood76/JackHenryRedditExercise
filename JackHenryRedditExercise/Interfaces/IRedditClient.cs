namespace JackHenryRedditExercise.Interfaces
{
    using System.Threading.Tasks;
    using JackHenryRedditExercise.Models;

    public interface IRedditClient
    {
        Task<SubRedditDetails?> GetDetails(string? subReddit = null, int? takeCount = 5);

        Task<IEnumerable<AuthorDetails>> GetTopAuthors(string? subReddit = null, int? takeCount = 5);
    }
}
