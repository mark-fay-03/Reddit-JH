using PostRetriever.WorkerService.Wrappers;
using Reddit.Data.Contracts;
using RedditSharp.Things;

namespace PostRetriever.WorkerService.Services;

public class PostObserver : IObserver<Post>
{
    private readonly DateTime _observationStartTime;
    private readonly IMapper<Post, IRedditPost> _postMapper;
    private readonly ILoggerWrapper<PostObserver> _logger;
    private readonly IConsoleWrapper _consoleWrapper;

    public PostObserver(IMapper<Post, IRedditPost> postMapper, ILoggerWrapper<PostObserver> logger, IDateTimeWrapper dateTimeWrapper, IConsoleWrapper consoleWrapper)
    {
        _postMapper = postMapper;
        _logger = logger;
        _consoleWrapper = consoleWrapper;
        _observationStartTime = dateTimeWrapper.UtcNow;
    }

    public void OnCompleted()
    {
        _consoleWrapper.WriteLine("Completed Post Observations");
    }

    public void OnError(Exception exception)
    {
        _consoleWrapper.WriteLine(exception.ToString());
        _logger.LogError(exception, "Could not process post.");
    }

    public async void OnNext(Post value)
    {
        try
        {
            if (value.CreatedUTC < _observationStartTime)
                return;
            
            _logger.LogInfo("Saving Post {Id}", value.Id);
            var redditPost = _postMapper.Map(value);
            await redditPost.Save();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not process Post {Id}", value.Id);
        }
    }
}