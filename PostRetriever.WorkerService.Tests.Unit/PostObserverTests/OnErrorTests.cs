using AutoFixture;
using NSubstitute;
using PostRetriever.WorkerService.Services;
using PostRetriever.WorkerService.Wrappers;
using Reddit.Data.Contracts;
using RedditSharp.Things;

namespace PostRetriever.WorkerService.Tests.Unit.PostObserverTests;

public class OnErrorTests
{
    private readonly IMapper<Post, IRedditPost> _mapper = Substitute.For<IMapper<Post, IRedditPost>>();
    private readonly ILoggerWrapper<PostObserver> _loggerWrapper = Substitute.For<ILoggerWrapper<PostObserver>>();
    private readonly IDateTimeWrapper _dateTimeWrapper = Substitute.For<IDateTimeWrapper>();
    private readonly IConsoleWrapper _consoleWrapper = Substitute.For<IConsoleWrapper>();
    private readonly Exception _exception = new Fixture().Create<Exception>();

    [Fact]
    [Trait("Category", "UnitTest")]
    public void ShouldOutputError()
    {
        // Act
        WhenOnCompletedIsCalled();

        // Assert
        ThenObservationCompletedWasOutput();
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void ShouldLogException()
    {
        // Act
        WhenOnCompletedIsCalled();

        // Assert
        ThenExceptionIsLogged();
    }

    private void WhenOnCompletedIsCalled()
    {
        IObserver<Post> target = new PostObserver(_mapper, _loggerWrapper, _dateTimeWrapper, _consoleWrapper);
        target.OnError(_exception);
    }

    private void ThenObservationCompletedWasOutput()
    {
        _consoleWrapper.Received().WriteLine(_exception.ToString());
    }

    private void ThenExceptionIsLogged()
    {
        _loggerWrapper.Received().LogError(_exception, "Could not process post.");
    }
}