using JackHenryRedditExercise.Models;

namespace JackHenryRedditExercise.Interfaces
{
    public interface IPostService 
    {
        string TopUpvotedPosts(SubRedditDetails details);
        string AuthorsWithMostPosts(IEnumerable<AuthorDetails> authors);
    }
}