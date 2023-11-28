using System.Collections.Concurrent;
using System.Reflection;
using AutoFixture;
using FluentAssertions;
using Reddit.Data.Contracts;

namespace Reddit.Data.InMemory.Tests.Unit
{
    public class RedditPostsTests
    {
        private static readonly Fixture Fixture = new();

        private readonly RedditPosts _redditPosts = Fixture.Create<RedditPosts>();
        
        private IRedditPost _redditPost = null!;

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task Add_ShouldAddPostToCollection()
        {
            // Arrange
            GivenAPost();
            
            // Act
            await WhenAddIsCalled();

            // Assert
            ThenPostWasAddedToCollection();
        }

        private void GivenAPost()
        {
            _redditPost = Fixture.Build<RedditPost>().FromFactory(()=> new RedditPost(_redditPosts)).Create();
        }

        private async Task WhenAddIsCalled()
        {
            await _redditPosts.Add(_redditPost);
        }


        private void ThenPostWasAddedToCollection()
        {
            var backingPostsField = typeof(RedditPosts).GetField("_posts", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
            backingPostsField.Should().NotBeNull();
            
            var backingPosts = backingPostsField.GetValue(_redditPosts) as ConcurrentBag<IRedditPost>;
            backingPosts.Should().NotBeNull();

            backingPosts.Should().Contain(_redditPost);
        }
    }
}