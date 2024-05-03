namespace JackHenryRedditExercise.Tests
{
    using JackHenryRedditExercise.Models;
    using JackHenryRedditExercise.Services;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Moq.Protected;
    using System.Net;
    using System.Text;
    using System.Text.Json;


    [TestClass]
    public class RedditClientServiceTests
    {
        private Mock<IHttpClientFactory> httpClientFactoryMock;
        private RedditClientService redditClient;

        [TestInitialize]
        public void Initialize()
        {
            this.httpClientFactoryMock = new Mock<IHttpClientFactory>();
            this.redditClient = new RedditClientService(this.httpClientFactoryMock.Object, Options.Create(new RedditClientConfiguration
            {
                UserAgent = "RedditClientServiceTests",
                Subreddit = "RedditClientServiceSubReddit"
            }));
        }

        [TestMethod]
        public async Task GetDetails_Success()
        {
            var expectedSubRedditDetails = new SubRedditDetails { Kind = "Listing_test" };
            var httpClient = CreateHttpClient(HttpStatusCode.OK, JsonSerializer.Serialize(expectedSubRedditDetails));
            this.httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var result = await redditClient.GetDetails();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedSubRedditDetails.Kind, result.Kind);
        }

        [TestMethod]
        public async Task GetDetails_Error()
        {
            var httpClient = CreateHttpClient(HttpStatusCode.InternalServerError, "Internal Server Error");
            this.httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => redditClient.GetDetails());
        }

        [TestMethod]
        public async Task GetTopAuthors_Success()
        {
            var expectedSubRedditDetails = new SubRedditDetails 
            { 
                Kind = "Listing_test",  
                Data = new SubRedditData
                {
                    Children = new List<T3Listing>
                    {
                        new T3Listing() { Data = new T3Data { Author = "Bill" } },
                        new T3Listing() { Data = new T3Data { Author = "Amy" } },
                        new T3Listing() { Data = new T3Data { Author = "Luke" } },
                        new T3Listing() { Data = new T3Data { Author = "Bill" } },
                        new T3Listing() { Data = new T3Data { Author = "Bill" } },
                        new T3Listing() { Data = new T3Data { Author = "Luke" } },
                        new T3Listing() { Data = new T3Data { Author = "Amy" } },
                        new T3Listing() { Data = new T3Data { Author = "Bill" } },
                        new T3Listing() { Data = new T3Data { Author = "Amy" } },
                        new T3Listing() { Data = new T3Data { Author = "Luke" } },
                        new T3Listing() { Data = new T3Data { Author = "Bill" } },
                        new T3Listing() { Data = new T3Data { Author = "Luke" } },
                        new T3Listing() { Data = new T3Data { Author = "Amy" } },
                        new T3Listing() { Data = new T3Data { Author = "Bill" } },
                        new T3Listing() { Data = new T3Data { Author = "Amy" } },
                        new T3Listing() { Data = new T3Data { Author = "Bill" } }
                    }
                }
            };

            var httpClient = CreateHttpClient(HttpStatusCode.OK, JsonSerializer.Serialize(expectedSubRedditDetails));
            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var result = await redditClient.GetTopAuthors();

            Assert.IsTrue(result.Count() == 3);
        }

        [TestMethod]
        public async Task GetTopAuthors_Error()
        {
            var httpClient = CreateHttpClient(HttpStatusCode.InternalServerError, "Internal Server Error");
            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => redditClient.GetTopAuthors());
        }

        private HttpClient CreateHttpClient(HttpStatusCode statusCode, string content)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content, Encoding.UTF8, "application/json")
                });

            return new HttpClient(handlerMock.Object);
        }
    }
}
