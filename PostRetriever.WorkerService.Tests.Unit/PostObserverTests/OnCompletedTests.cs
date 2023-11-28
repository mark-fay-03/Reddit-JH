using NSubstitute;
using PostRetriever.WorkerService.Services;
using PostRetriever.WorkerService.Wrappers;
using Reddit.Data.Contracts;
using RedditSharp.Things;

namespace PostRetriever.WorkerService.Tests.Unit.PostObserverTests;

public class OnCompletedTests
{
    private readonly IMapper<Post, IRedditPost> _mapper = Substitute.For<IMapper<Post, IRedditPost>>();
    private readonly ILoggerWrapper<PostObserver> _logger = Substitute.For<ILoggerWrapper<PostObserver>>();
    private readonly IDateTimeWrapper _dateTimeWrapper = Substitute.For<IDateTimeWrapper>();
    private readonly IConsoleWrapper _consoleWrapper = Substitute.For<IConsoleWrapper>();

    [Fact]
    [Trait("Category", "UnitTest")]
    public void ShouldOutputObservationCompleted()
    {
        // Act
        WhenOnCompletedIsCalled();

        // Assert
        ThenObservationCompletedWasOutput();
    }

    private void WhenOnCompletedIsCalled()
    {
        IObserver<Post> target = new PostObserver(_mapper, _logger, _dateTimeWrapper, _consoleWrapper);
        target.OnCompleted();
    }

    private void ThenObservationCompletedWasOutput()
    {
        _consoleWrapper.Received().WriteLine("Completed Post Observations");
    }
}