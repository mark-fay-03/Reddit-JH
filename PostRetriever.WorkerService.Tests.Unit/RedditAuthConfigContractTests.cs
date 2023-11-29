using FluentAssertions;
using PostRetriever.WorkerService.Services;

namespace PostRetriever.WorkerService.Tests.Unit
{
    public class RedditAuthConfigContractTests
    {
        private class ExpectedRedditAuthConfig
        {
            // ReSharper disable UnusedMember.Local
            public string ClientId { get; set; } = null!;
            public string ClientSecret { get; set; } = null!;
            public string UserName { get; set; } = null!;
            public string Password { get; set; } = null!;
            public string UserAgent { get; set; } = null!;
            public string RedirectUri { get; set; } = null!;
            public string SubRedditName { get; set; } = null!;
            // ReSharper restore UnusedMember.Local
        }

        [Fact]
        public void ShouldBeProperContract()
        {
            var properties = typeof(RedditAuthConfig).GetProperties().Select(p => new {p.Name, p.PropertyType}).ToArray();
            var expectedProperties = typeof(ExpectedRedditAuthConfig).GetProperties().Select(p => new { p.Name, p.PropertyType }).ToArray();

            properties.Should().BeEquivalentTo(expectedProperties);
        }
    }
}