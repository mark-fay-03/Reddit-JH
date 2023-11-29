using Microsoft.AspNetCore.Mvc;
using StatisticsAPI.Services;

namespace StatisticsAPI.Controllers;

public class PostsController : Controller
{
    private readonly IPostsService _postsService;
    private readonly ILoggerWrapper<PostsController> _logger;

    public PostsController(IPostsService postsService, ILoggerWrapper<PostsController> logger)
    {
        _postsService = postsService;
        _logger = logger;
    }

    [HttpGet("ByVotes")]
    public IResult ByVotes([FromQuery]int top)
    {
        try
        {
            if (top <= 0 || top > 100)
            {
                return Results.BadRequest("Invalid number of users requested");
            }
            
            var topVotedPosts = _postsService.GetTopVoted(top);
            
           
            return Results.Ok(topVotedPosts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected Error Retrieving {Class}.{Method}", nameof(PostsController), nameof(ByVotes));
            var internalServerErrorResponse = Results.Problem("Internal Server Error", statusCode: 500);
            return internalServerErrorResponse;
        }
    }
}