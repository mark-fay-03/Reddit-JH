using Microsoft.AspNetCore.Mvc;
using StatisticsAPI.Services;

namespace StatisticsAPI.Controllers;

public class UsersController : Controller
{
    private readonly IUsersService _usersService;
    private readonly ILoggerWrapper<UsersController> _logger;

    public UsersController(IUsersService usersService, ILoggerWrapper<UsersController> logger)
    {
        _usersService = usersService;
        _logger = logger;
    }

    [HttpGet("ByPosts")]
    public IResult ByPosts([FromQuery] int top)
    {
        try
        {
            if (top <= 0 || top > 100)
            {
                return Results.BadRequest("Invalid number of users requested");
            }

            var topAuthors = _usersService.GetTopPosters(top);

            return Results.Ok(topAuthors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected Error Retrieving {Class}.{Method}", nameof(UsersController), nameof(ByPosts));
            var internalServerErrorResponse = Results.Problem("Internal Server Error", statusCode: 500);
            return internalServerErrorResponse;
        }
    }
}