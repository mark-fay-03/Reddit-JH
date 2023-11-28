using System.Diagnostics.CodeAnalysis;
using PostRetriever.WorkerService.Services;
using RedditSharp.Things;

namespace PostRetriever.WorkerService
{
    [ExcludeFromCodeCoverage(Justification = "The background service is not feasibly unit testable")]
    public class PostsProcessorService : BackgroundService
    {
        private readonly IObserver<Post> _postObserver;
        private readonly IRedditPostProcessor _redditPostProcessor;

        public PostsProcessorService(IObserver<Post> postObserver, IRedditPostProcessor redditPostProcessor)
        {
            _postObserver = postObserver;
            _redditPostProcessor = redditPostProcessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _redditPostProcessor.StreamPosts("funny", _postObserver, stoppingToken);
        }
    }
}