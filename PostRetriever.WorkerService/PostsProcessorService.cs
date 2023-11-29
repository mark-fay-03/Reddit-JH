using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using PostRetriever.WorkerService.Services;
using RedditSharp.Things;

namespace PostRetriever.WorkerService
{
    [ExcludeFromCodeCoverage(Justification = "The background service is not feasibly unit testable")]
    public class PostsProcessorService : BackgroundService
    {
        private readonly IObserver<Post> _postObserver;
        private readonly IRedditPostProcessor _redditPostProcessor;
        private readonly RedditAuthConfig _redditAuthConfig;

        public PostsProcessorService(IObserver<Post> postObserver, IRedditPostProcessor redditPostProcessor, IOptions<RedditAuthConfig> redditAuthConfig)
        {
            _postObserver = postObserver;
            _redditPostProcessor = redditPostProcessor;
            _redditAuthConfig = redditAuthConfig.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _redditPostProcessor.StreamPosts(_redditAuthConfig.SubRedditName, _postObserver, stoppingToken);
        }
    }
}