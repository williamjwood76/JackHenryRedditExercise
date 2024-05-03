namespace JackHenryRedditExercise.Services
{
    using JackHenryRedditExercise.Interfaces;
    using JackHenryRedditExercise.Models;

    public class RedditPostService : IPostService
    {
        private string? retVal;

        public string TopUpvotedPosts(SubRedditDetails details)
        {
            retVal = "";
            if (details != null)
            {
                var data = details.Data.Children.First().Data;

                if (data != null)
                {
                    retVal = $"Posts with most upvotes";
                    retVal += $"\r\n";
                    retVal += $"------------------------";
                    retVal += $"\r\n";
                }

                foreach (var detail in details.Data.Children)
                {
                    if (detail.Data != null)
                    {
                        retVal += $"{detail.Data.Ups} Upvotes for post '{detail.Data.Title}'";
                        retVal += $"\r\n";
                    }
                }
            }
            return retVal;
        }

        public string AuthorsWithMostPosts(IEnumerable<AuthorDetails> authors)
        {
            retVal = "";
            if (authors != null)
            {
                retVal = $"Users with most Posts";
                retVal += $"\r\n";
                retVal += $"----------------------";
                retVal += $"\r\n";
                foreach (var author in authors)
                {
                    retVal += $"{author.Name} posted {author.PostCount} times";
                    retVal += $"\r\n";
                }
            }
            return retVal;
        }
    }
}