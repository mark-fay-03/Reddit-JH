using FluentAssertions;
using Reddit.Data.Contracts;

namespace Reddit.Data.InMemory.Tests.Unit
{
    public class RedditPostContractTests
    {
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
            var properties = typeof(RedditPost).GetProperties().Select(p => new {p.Name, p.PropertyType}).ToArray();
            var expectedProperties = typeof(ExpectedRedditPost).GetProperties().Select(p => new { p.Name, p.PropertyType }).ToArray();

            properties.Should().BeEquivalentTo(expectedProperties);
        }
    }
}