using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Reddit.Data.Contracts;

namespace Reddit.Data.InMemory.Tests.Unit
{
    public class RedditPostTests
    {
        private static readonly Fixture Fixture = new();

        private readonly IRedditPosts _redditPosts = Substitute.For<IRedditPosts>();
        
        private IRedditPost _redditPost = null!;
        
        private readonly Exception _exception = Fixture.Create<Exception>();
        private Func<Task> _action = null!;

        private class ExpectedRedditPost
        {
            // ReSharper disable UnusedMember.Local
            public string Id { get; set; } = null!;
            public DateTime Created { get; set; }
            public string AuthorName { get; set; } = null!;
            public int UpVotes { get; set; }
            // ReSharper restore UnusedMember.Local
        }

        [Fact]
        public void ShouldBeProperContract()
        {
            var properties = typeof(RedditPost).GetProperties().Select(p => new { p.Name, p.PropertyType }).ToArray();
            var expectedProperties = typeof(ExpectedRedditPost).GetProperties().Select(p => new { p.Name, p.PropertyType }).ToArray();

            properties.Should().BeEquivalentTo(expectedProperties);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task Save_ShouldAddPostToCollection()
        {
            // Arrange
            GivenAPost();
            
            // Act
            await WhenSaveIsCalled();

            // Assert
            ThenPostWasAddedToCollection();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task Save_ShouldBubbleUpExceptions()
        {
            // Arrange
            GivenAPost();
            GivenAnExceptionIsThrown();

            // Act
            WhenSaveThrowsAnException();

            // Assert
            await ThenExceptionBubbledUp();
        }


        private void GivenAPost()
        {
            _redditPost = Fixture.Build<RedditPost>().FromFactory(()=> new RedditPost(_redditPosts)).Create();
        }

        private void GivenAnExceptionIsThrown()
        {
            _redditPosts.Add(Arg.Any<IRedditPost>()).ThrowsAsync(_exception);
        }

        private async Task WhenSaveIsCalled()
        {
            await _redditPost.Save();
        }

        private void WhenSaveThrowsAnException()
        {
            _action = WhenSaveIsCalled;
        }

        private void ThenPostWasAddedToCollection()
        {
            _redditPosts.Received().Add(_redditPost);
        }

        private async Task ThenExceptionBubbledUp()
        {
            var thrownException = await _action.Should().ThrowAsync<Exception>();
            thrownException.Which.Should().Be(_exception);
        }
    }
}