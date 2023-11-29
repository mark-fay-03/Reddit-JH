using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Reddit.Data.Contracts;
using StatisticsAPI.Mappers;
using StatisticsAPI.Models;

namespace StatisticsAPI.Tests.Unit.MapperTests;

public class PostMapperTests
{
    private Post _domainModel = null!;
    private IRedditPost _dataModel = null!;

    [Fact]
    [Trait("Category", "UnitTest")]
    public void ShouldMapDataToDomainModel()
    {
        // Arrange
        GivenADataModel();

        // Act
        WhenMapIsCalled();

        // Assert
        ThenDomainModelIsMapped();
    }

    private void GivenADataModel()
    {
        _dataModel = new Fixture()
            .Build<RedditPost>()
            .FromFactory(() => new RedditPost(Substitute.For<IRedditPosts>()))
            .Create();
    }

    private void WhenMapIsCalled()
    {
        IMapper<Post, IRedditPost> target = new PostMapper();
        _domainModel = target.Map(_dataModel);
    }

    private void ThenDomainModelIsMapped()
    {
        _domainModel.Id.Should().Be(_dataModel.Id);
        _domainModel.AuthorName.Should().Be(_dataModel.AuthorName);
        _domainModel.UpVotes.Should().Be(_dataModel.UpVotes);
        _domainModel.Created.Should().Be(_dataModel.Created);
    }
}